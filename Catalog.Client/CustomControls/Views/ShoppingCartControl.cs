using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;

using Catalog.Common.Repository;
using Catalog.Common.Service;
using Catalog.Client.Properties;
using Catalog.Common.Utils;

using FilterPredicate = System.Linq.Expressions.Expression<System.Func<Catalog.Common.Service.ShoppingCart, bool>>;


namespace Catalog.Client
{
	class ShoppingCartControl : BaseGridControl
	{
		#region Properties

		Action<DbQuery<ShoppingCart>> QueryCallback { get; set; }

		VirtualGridViewInfo ChildViewInfo { get; set; }

		RadButton AcceptButton { get; set; }

		Action RemoveCartCallback { get; set; }

		Action<int, Inventory> CartItemCallback { get; set; }

		#endregion

		#region Initializers

		public ShoppingCartControl() : base(null)
		{
			AcceptButton = new RadButton
			{
				Text = "Отправить",
				BackColor = SystemColors.ButtonFace,
				Dock = DockStyle.Fill,
				Font = new Font("Microsoft Sans Serif", 9F),
				Image = Resources.ok,
				Name = "sendButton",
				Size = new Size(114, 39),
				TabIndex = 4,
				TextImageRelation = TextImageRelation.ImageBeforeText,
			};
			this.GridControl.MasterViewInfo.MinColumnWidth = 100;
			var ctrl = this.radTableLayoutPanel.GetControlFromPosition(0, 0);
			this.radTableLayoutPanel.Controls.Remove(ctrl);
			this.radTableLayoutPanel.Controls.Add(AcceptButton, 2, 0);

			AcceptButton.Click += AcceptButton_Click;
		}

		protected override void Initialize()
		{
			this.exportFileName = "Корзина.xlsx";
			columnTypes = new Type[]
			{
				typeof(byte),
				typeof(int),
				typeof(decimal),
				typeof(DateTime),
				typeof(DateTime),
			};

			this.columnNames.Add("Статус");
			this.columnNames.Add("Общее количество");
			this.columnNames.Add("Общая сумма");
			this.columnNames.Add("Дата изменения");
			this.columnNames.Add("Удалить все");

			this.GridControl.ColumnCount = columnNames.Count;

			this.detailColumnNames.Add("Фото");
			this.detailColumnNames.Add("Артикул");
			this.detailColumnNames.Add("Код");
			this.detailColumnNames.Add("Бренд");
			this.detailColumnNames.Add("Наименование товара");
			this.detailColumnNames.Add("Цена за единицу");
			this.detailColumnNames.Add("Дата изменения");
			this.detailColumnNames.Add("Дата добавления");
			this.detailColumnNames.Add("Количество");
			this.detailColumnNames.Add("Удалить");

			this.GridControl.MasterViewInfo.SetColumnDataType(columnTypes);

			this.QueryCallback = query =>
			{
				this.data = query.ToList();
				this.GridControl.RowCount = this.data.Count;

				if (data.Count > 0)
				{
					this.GridControl.ColumnCount = columnNames.Count;
					this.DataLoaded = true;
					RefreshDetailsData();
				}
				else GridControl.ColumnCount = 0;

				this.GridControl.MasterViewInfo.AllowColumnResize = false;
				this.GridControl.MasterViewInfo.IsWaiting = false;

				this.AcceptButton.Enabled = data.Count > 0;
				this.ExportButton.Enabled = data.Count > 0;


				this.GridControl.TableElement.SynchronizeRows(true, true);
			};

			RemoveCartCallback = () =>
			{
				if (data.Count == 0)
					return;
				var cart = data[0];
				var cartItems = detailsData;

				if (cartItems.Count > 0)
				{
					foreach (var cartItem in cartItems)
					{
						cartItem.Quantity = 0;
					}

					cart.TotalQuantity = 0;
					cart.TotalPrice = 0M;
					cart.Status = (byte)(ShoppingCartStatus.Pending);

					EntityModel.Commit();

					var shoppingStatus = String.Format(
						StatusTemplate.ORDERS_STATUS_TEMPLATE,
						cart.TotalQuantity,
						CultureConfig.CurrencyInfo.Format(cart.TotalPrice)
					);

					Invoke(new Action(() => TopButton.Text = shoppingStatus));
				}

				RefreshData();
			};

			CartItemCallback = (rowIndex, inventory) =>
			{
				try
				{
					var cart = data[0];
					var cartItem = detailsData[rowIndex];

					if (cartItem != null)
					{
						if (inventory != null)
						{
							cart.TotalQuantity = Inventory.ShoppingCart.TotalQuantity;
							cart.TotalPrice = Inventory.ShoppingCart.TotalPrice;
							cartItem.Quantity = inventory.Quantity;
						} else 
						{
							cart.TotalQuantity -= cartItem.Quantity;
							cart.TotalPrice -= cartItem.Quantity * cartItem.UnitPrice;
							cartItem.Quantity = 0;
						}

						cart.Status = (byte) ShoppingCartStatus.Pending;

						var shoppingStatus = String.Format(
							StatusTemplate.ORDERS_STATUS_TEMPLATE,
							cart.TotalQuantity,
							CultureConfig.CurrencyInfo.Format(cart.TotalPrice)
						);

						TopButton.Text = shoppingStatus;
						MainForm.StatusLabelElement.Text = shoppingStatus;

						EntityModel.Commit();

						RefreshDetailsData();
						if (detailsData.Count == 0)
							RefreshData();

						GridControl.TableElement.SynchronizeRows();
					}
				}
				catch
				{ }
			};

			this.GridControl.QueryHasChildRows += GridControl_QueryHasChildRows;
			this.GridControl.CellFormatting += GridControl_CellFormatting;
			this.GridControl.RowExpanding += GridControl_RowExpanding;
			this.GridControl.ScreenTipNeeded += GridControl_ScreenTipNeeded;
		}

		private void SendMail(TransactionHistory transaction)
		{
			var formatProvider = new XlsxFormatProvider();
			var workbook = CreateWorkbook();
			var data = formatProvider.Export(workbook);

			string message = $"Заказ #{transaction.TransactionID}\nID клиента: 0\n";

			WebRepository.SendMail(data, exportFileName, message);
		}

		#endregion

		#region Data handlers

		public override void RefreshData()
		{
			if (MainForm.StatusLabelElement != null)
				MainForm.StatusLabelElement.Text = "Обновление корзины";
			data.Clear();
			detailsData.Clear();
			GridControl.MasterViewInfo.IsWaiting = true;
			FilterDataAsync(c => c.TotalQuantity > 0);
		}

		protected void RefreshDetailsData()
		{
			this.detailsData = data[0].ShoppingCartItems
									  .Where(c => c.Quantity > 0)
									  .ToList();
			if (detailsData.Count > 0)
			{
				if (ChildViewInfo is null)
				{
					ChildViewInfo = GridControl.MasterViewInfo.GetChildViewInfo(0, true);
					this.ChildViewInfo.AllowColumnResize = false;
					this.ChildViewInfo.MinRowHeight = this.GridControl.MasterViewInfo.MinRowHeight;
				}
				ChildViewInfo.ColumnCount = detailColumnNames.Count;
				ChildViewInfo.RowCount = detailsData.Count;

				ChildViewInfo.AllowColumnResize = false;
				GridControl.MasterViewInfo.ExpandRow(0);
				GridControl.MasterViewInfo.Padding = new Padding(0, 100, 0, 100);
				ChildViewInfo.AutoSizeColumnsMode = VirtualGridAutoSizeColumnsMode.Fill;
				cellElements.ForEach(el => el.CartItems = detailsData);
			}
			else ChildViewInfo = null;
		}

		private void FilterDataAsync(FilterPredicate predicate)
		{
			var task = Task.Run(() =>
			{
				var cart = Repository.Context.ShoppingCarts
											 .Include(c => c.ShoppingCartItems)
											 .Where(predicate);
				var query = FilterHelper.Filter(cart, GridControl.FilterDescriptors);
				return query;
			});

			ExecuteQueryAsync(task, QueryCallback);
		}

		#endregion

		#region Event handlers

		private void OnLoad(Object sender, EventArgs e)
		{
			base.OnLoad(e);
			RefreshData();
		}

		private async void AcceptButton_Click(Object sender, EventArgs e)
		{
			if (Properties.Settings.AskConfirmation)
			{
				var dialogResult = RadMessageBox.Show(
					"Хотите продолжить?",
					"<html><b>Подтверждение</b>",
					MessageBoxButtons.OKCancel
				);

				if (dialogResult == DialogResult.Cancel)
					return;
			}

			var cart = data[0];

			if (!WebRepository.HasConnection)
			{
				RadMessageBox.Show(
								"Отсутствует соединение с сетью, пожалуйста, подключитесь к сети и" +
								" повторите попытку снова!",
								"<html><b>Подключение</b>"
							);

				cart.Status = (byte)ShoppingCartStatus.Pending;
				cart.Save(false, true);
				return;
			}

			cart.Status = (byte)ShoppingCartStatus.Ordered;

			await Task.Run(() =>
			{
				var transactionHistory = new TransactionHistory()
				{
					TransactionDate = DateTime.Now,
					Quantity = cart.TotalQuantity,
					TotalCost = cart.TotalPrice,
					ModifiedDate = DateTime.Now
				};

				Repository.Context.TransactionHistories.Add(transactionHistory);

				Repository.SaveChanges(
					() => RadMessageBox.Show(
							$"Заказ №{transactionHistory.TransactionID} принят.\nЖдите подтверждение оператора.",
							caption: "Успех!",
							buttons: MessageBoxButtons.OK
						)
				);

				SendMail(transactionHistory);
			});
			RemoveCartCallback();
		}

		protected override void GridControl_CreateCellElement(object sender, VirtualGridCreateCellEventArgs e)
		{
			if (e.ViewInfo != this.GridControl.MasterViewInfo) // ChildViewInfo
			{
				if (detailsData.Count > 0 && data.Count > 0)
				{
					if (e.RowIndex >= 0 && e.RowIndex < detailsData.Count)
					{
						var rowData = data[0];
						switch (e.ColumnIndex)
						{
							case 0:
								e.CellElement = new ProductPhotoCellElement();
								break;
							case 8:
								var quantityCellElement = new QuantityCellElement(ProductInventories, e.ColumnIndex)
								{
									CartItems = detailsData,
									Callback = (index, inventory) =>
									{
										CartItemCallback(index, inventory);
									}
								};
								e.CellElement = quantityCellElement;
								this.cellElements.Add(quantityCellElement);
								break;
							case 9:
								e.CellElement = new RemoveCellElement(e.ColumnIndex)
								{
									Callback = index =>
									{
										CartItemCallback(index, null);
									}
								};
								break;
						}
					}
				}
			}
			else // MasterViewInfo
			{
				if (data.Count > e.RowIndex && e.RowIndex >= 0)
				{
					switch (e.ColumnIndex)
					{
						case 4:
							var removeCellElement = new RemoveCellElement(e.ColumnIndex)
							{
								Callback = index => 
								{
									RemoveCartCallback.Invoke();
								}
							};
							e.CellElement = removeCellElement;
							break;
					}
				}
			}

			base.GridControl_CreateCellElement(sender, e);
		}

		protected override void GridControl_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
		{
			base.GridControl_CellValueNeeded(sender, e);

			if (e.ColumnIndex < 0 || data.Count == 0)
				return;

			if (e.ViewInfo == this.GridControl.MasterViewInfo)  // MasterViewInfo
			{
				if (e.RowIndex < 0)
				{
					e.FieldName = FieldsHelper.ShoppingCartFields[e.ColumnIndex];
				}
				if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
				{
					e.Value = columnNames[e.ColumnIndex];
					e.FormatString = "<html><b>{0}";
					e.FieldName = FieldsHelper.ShoppingCartFields[e.ColumnIndex];
				}
				if (e.RowIndex >= 0 && data.Count > 0)
				{
					var rowData = data[0];
					switch (e.ColumnIndex)
					{
						case 0:
							e.Value = rowData.Status == (byte)ShoppingCartStatus.Pending ? "Ожидает" : "Заказан";
							break;
						case 1:
							e.Value = rowData.TotalQuantity;
							break;
						case 2:
							var price = rowData.TotalPrice;
							e.Value = price;
							e.FormatString = CultureConfig.CurrencyInfo.Format(price);
							break;
						case 3:
							e.Value = rowData.ModifiedDate;
							e.FormatString = CultureConfig.DateTimeInfo.Format(rowData.ModifiedDate);
							break;
					}
				}
			}
			else // ChildViewInfo
			{
				if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
				{
					e.Value = detailColumnNames[e.ColumnIndex];
					e.FormatString = "<html><b>{0}";
				}
				if (e.RowIndex >= 0 && e.RowIndex < detailsData.Count)
				{
					if (detailsData.Count > 0)
					{
						var rowData = detailsData[e.RowIndex];
						var inventory = MainRepository.ProductInventoriesCache
													  .Where(pi => pi.ProductID == rowData.ProductID)
													  .SingleOrDefault();
						var photo = Properties.Settings.LoadImages
												? MainRepository.ProductPhotoCache.Find(p => p.ProductID == rowData.ProductID)
												: null;

						Image image = ImageUtils.GetImage(photo?.ThumbNailPhoto, Resources.placeholder);

						switch (e.ColumnIndex)
						{
							case 0:
								e.Value = image;
								break;
							case 1:
								e.Value = inventory.Product.ArticleNumber;
								break;
							case 2:
								e.Value = inventory.Product.Code;
								break;
							case 3:
								e.Value = inventory.Product.Brand;
								break;
							case 4:
								e.Value = inventory.Product.Name;
								break;
							case 5:
								e.Value = rowData.UnitPrice;
								e.FormatString = CultureConfig.CurrencyInfo.Format(rowData.UnitPrice);
								break;
							case 6:
								e.Value = rowData.ModifiedDate;
								e.FormatString = CultureConfig.DateTimeInfo.Format(rowData.ModifiedDate);
								break;
							case 7:
								e.Value = rowData.DateCreated;
								e.FormatString = CultureConfig.DateTimeInfo.Format(rowData.DateCreated);
								break;
							case 8:
								var qnty = ProductInventories.Get(rowData.ProductID);
								e.Value = qnty.Quantity;
								break;
						}
					}
				}
			}
		}

		protected void GridControl_CellFormatting(object sender, VirtualGridCellElementEventArgs e)
		{
			if (e.CellElement is VirtualGridHeaderCellElement)
			{
				var cell = e.CellElement as VirtualGridHeaderCellElement;
				cell.TextWrap = true;
				cell.Arrow.ShouldPaint = false;
			}
			if (e.CellElement is VirtualGridFilterCellElement)
			{
				if (e.CellElement.ColumnIndex == 0
				|| e.CellElement.ColumnIndex == 9
				|| e.CellElement.ColumnIndex == 8)
				{
					e.CellElement.Enabled = false;
				}
				else
				{
					e.CellElement.ResetValue(RadItem.EnabledProperty, ValueResetFlags.Local);

				}
			}

			if (e.ViewInfo != this.GridControl.MasterViewInfo)
			{
				if (e.CellElement.RowIndex >= 0)
				{
					switch (e.CellElement.ColumnIndex)
					{
						case 0:
							{
								// ProductPhoto
								if (e.CellElement.Value is Image image)
								{
									e.CellElement.Text = String.Empty;
									e.CellElement.Image = image;
								}

							}
							break;
						default:
							e.CellElement.TextWrap = true;
							e.CellElement.Image = null;
							break;
					}
				}
			}
		}

		private void GridControl_QueryHasChildRows(object sender, VirtualGridQueryHasChildRowsEventArgs e)
		{
			e.HasChildRows = e.ViewInfo == this.GridControl.MasterViewInfo;
		}

		private void GridControl_RowExpanding(object sender, VirtualGridRowExpandingEventArgs e)
		{
			this.GridControl.TableElement.SynchronizeRows();
		}

		protected void GridControl_ScreenTipNeeded(Object sender, ScreenTipNeededEventArgs e)
		{
			//var cell = e.Item as VirtualGridCellElement;
			if (e.Item is VirtualGridCellElement cell
				&& cell.ViewInfo != GridControl.MasterViewInfo)
			{
				var cellIndex = cell.RowIndex;
				if (cell != null && cellIndex >= 0)
				{
					if (cell.ColumnIndex == 0)
					{
						var inventory = MainRepository.ProductInventoriesCache
										.Where(p => p.ProductID == detailsData[cellIndex].ProductID)
										.SingleOrDefault();
						ShowScreenTipForCell(cell, cellIndex, inventory);
					}
				}
			}
		}

		protected override void GridControl_SortChanged(object sender, VirtualGridEventArgs e)
		{
			if (e.ViewInfo.SortDescriptors.Count == 0)
				return;

			var propertyName = e.ViewInfo.SortDescriptors[0].PropertyName;

			if (propertyName == "OrderStatus")
				return;
		}

		#endregion

		#region Export handler

		protected override Workbook CreateWorkbook()
		{
			var workbook = new Workbook();
			Worksheet worksheet = workbook.Worksheets.Add();

			void alignCell(CellSelection selection)
			{
				selection.SetVerticalAlignment(
					Telerik.Windows.Documents.Spreadsheet.Model.RadVerticalAlignment.Center
				);
				selection.SetHorizontalAlignment(
					Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Center
				);
			}

			var cartItemColumnNames = new List<String>(detailColumnNames);
			cartItemColumnNames.Remove("Фото");
			cartItemColumnNames.Remove("Дата добавления");
			cartItemColumnNames.Remove("Удалить");

			// set header
			for (int i = 0; i < cartItemColumnNames.Count; i++)
			{
				CellSelection selection = worksheet.Cells[0, i];
				selection.SetValue(cartItemColumnNames[i]);
				selection.SetIsBold(true);
				alignCell(selection);
			}

			for (int i = 0; i < detailsData.Count; i++)
			{
				int rowIndex = i + 1;
				var cartItem = detailsData[i];
				var product = MainRepository.ProductsCache
											.Where(p => p.ProductID == cartItem.ProductID)
											.SingleOrDefault();
				CellSelection selection = worksheet.Cells[rowIndex, 0];
				selection.SetValue(product?.ArticleNumber);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 1];
				selection.SetValue(product?.Code);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 2];
				selection.SetValue(product?.Brand);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 3];
				selection.SetValue(product?.Name);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 4];
				selection.SetValue(cartItem.UnitPrice.ToString());
				selection.SetFormat(new CellValueFormat("## ###.## ₽"));
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 5];
				selection.SetValue(cartItem.ModifiedDate.ToString());
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 6];
				selection.SetValue(cartItem.Quantity);
				alignCell(selection);
			}

			var cart = data[0];

			for (int i = detailsData.Count; i < detailsData.Count + 1; i++)
			{
				int rowIndex = i + 1;

				CellSelection selection = worksheet.Cells[rowIndex, 4];
				selection.SetValue(cart.TotalPrice.ToString());
				selection.SetFormat(new CellValueFormat("## ###.## ₽"));
				selection.SetIsBold(true);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 5];
				selection.SetValue(cart.ModifiedDate.ToString());
				selection.SetIsBold(true);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 6];
				selection.SetValue(cart.TotalQuantity);
				selection.SetIsBold(true);
				alignCell(selection);
			}

			worksheet.Columns[worksheet.UsedCellRange].AutoFitWidth();
			worksheet.Name = "Корзина";
			return workbook;
		}

		#endregion

		#region Private members

		List<ShoppingCart> data = new List<ShoppingCart>();

		List<ShoppingCartItem> detailsData = new List<ShoppingCartItem>();

		readonly List<QuantityCellElement> cellElements = new List<QuantityCellElement>();

		#endregion
	}
}
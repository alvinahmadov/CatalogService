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

using Catalog.Common;
using Catalog.Common.Repository;
using Catalog.Common.Service;
using Catalog.Client.Properties;

using FilterPredicate = System.Linq.Expressions.Expression<System.Func<Catalog.Common.Service.ShoppingCart, System.Boolean>>;
using System.Diagnostics;

namespace Catalog.Client
{
	sealed class ShoppingCartControl : BaseGridControl
	{
		#region Properties

		VirtualGridViewInfo ChildViewInfo { get; set; }

		//RadButton AcceptButton { get; set; }

		/// <summary>
		/// Removes cart and it's items from list
		/// </summary>
		Action RemoveCartCallback { get; set; }

		/// <summary>
		/// Removes cart item from list
		/// </summary>
		Action<int, GridItem> CartItemCallback { get; set; }

		/// <summary>
		/// Used to fill cart with products
		/// </summary>
		Action<DbQuery<ShoppingCart>> QueryCallback { get; set; }

		#endregion

		#region Initializers

		public ShoppingCartControl()
		{
			this.radAcceptButton = new RadButton
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
			var ctrl = this.radTableLayoutPanel.GetControlFromPosition(0, 0);
			this.radTableLayoutPanel.Controls.Remove(ctrl);
			this.radTableLayoutPanel.Controls.Add(this.radAcceptButton, 2, 0);
			this.radAcceptButton.Click += AcceptButton_Click;
			this.GridControl.MasterViewInfo.MinColumnWidth = 100;
		}

		protected override void Initialize()
		{
			this.exportFileName = "Корзина.xlsx";
			this.columnNames = ColumnNamesHelper.ShoppingCart;
			this.detailColumnNames = ColumnNamesHelper.ShoppingCartItem;
			this.GridControl.ColumnCount = columnNames.Count;
			this.GridControl.MasterViewInfo.SetColumnDataType(ColumnTypesHelper.ShoppingCart);

			/// Used to delete cart and its items from list
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
					Entity.Commit();

					var shoppingStatus = String.Format(
						MESSAGE.Gui.ORDERS_STATUS_TEMPLATE,
						cart.TotalQuantity,
						CultureConfig.CurrencyInfo.Format(cart.TotalPrice)
					);

					Invoke(new Action(() => TopButton.Text = shoppingStatus));
				}

				RefreshData();
			};

			/// Used to delete cart item from list
			CartItemCallback = (rowIndex, gridItem) =>
			{
				try
				{
					var cart = data[0];
					var cartItem = detailsData[rowIndex];

					if (cartItem != null)
					{
						if (gridItem != null)
						{
							cart.TotalQuantity = GridItem.ShoppingCart.TotalQuantity;
							cart.TotalPrice = GridItem.ShoppingCart.TotalPrice;
							cartItem.Quantity = gridItem.Quantity;
						}
						else
						{
							cart.TotalQuantity -= cartItem.Quantity;
							cart.TotalPrice -= cartItem.Quantity * cartItem.UnitPrice;
							cartItem.Quantity = 0;
						}

						cart.Status = (byte)ShoppingCartStatus.Pending;

						var shoppingStatus = String.Format(
							MESSAGE.Gui.ORDERS_STATUS_TEMPLATE,
							cart.TotalQuantity,
							CultureConfig.CurrencyInfo.Format(cart.TotalPrice)
						);

						TopButton.Text = shoppingStatus;
						MainForm.StatusLabelElement.Text = shoppingStatus;
						Entity.Commit();

						RefreshDetailsData();
						if (detailsData.Count == 0)
							RefreshData();

						GridControl.TableElement.SynchronizeRows();
					}
				}
				catch
				{ }
			};

			/// Used to fill cart with products
			QueryCallback = query =>
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

				this.radAcceptButton.Enabled = data.Count > 0;
				this.radExportButton.Enabled = data.Count > 0;

				this.GridControl.MasterViewInfo.AllowColumnResize = false;
				this.GridControl.MasterViewInfo.IsWaiting = false;
				this.GridControl.TableElement.SynchronizeRows(true, true);
			};

			this.GridControl.QueryHasChildRows += GridControl_QueryHasChildRows;
			this.GridControl.CellFormatting += GridControl_CellFormatting;
			this.GridControl.RowExpanding += GridControl_RowExpanding;
			this.GridControl.ScreenTipNeeded += GridControl_ScreenTipNeeded;
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

		private void RefreshDetailsData()
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
				cellElements.ForEach(el => el.productCartItems = detailsData);
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

		/// <summary>
		/// Creates an Workbook file for sending products list as attached file in mail
		/// </summary>
		/// <param name="transaction">
		///		Transaction item
		/// </param>
		/// 
		private void SendMail(TransactionHistory transaction)
		{
			var workbook = CreateWorkbook();
			var formatProvider = new XlsxFormatProvider();
			byte[] data = formatProvider.Export(workbook);
			string message = String.Format(MESSAGE.Mail.BODY, transaction.TransactionID, 0);

			Mail.Send(data, exportFileName, message);
		}

		#endregion

		#region Event handlers

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			RefreshData();
		}

		protected override void GridControl_CreateCellElement(Object sender, VirtualGridCreateCellEventArgs e)
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
								e.CellElement = new PhotoCellElement();
								break;
							case 8:
								var quantityCellElement = new GridElement(ProductInventories, e.ColumnIndex)
								{
									productCartItems = detailsData,
									Callback = (index, gridItem) =>
									{
										CartItemCallback(index, gridItem);
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

		protected override void GridControl_CellValueNeeded(Object sender, VirtualGridCellValueNeededEventArgs e)
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
						var inventory = MainRepository.InventoriesCache
													  .Where(pi => pi.ProductID == rowData.ProductID)
													  .SingleOrDefault();
						var photo = Properties.Settings.Default.LoadImage
												? MainRepository.PhotoCache.Find(p => p.ProductID == rowData.ProductID)
												: null;

						Image image = ImageUtil.GetImage(photo?.ThumbNailPhoto, Resources.placeholder);

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

		protected override void GridControl_SortChanged(Object sender, VirtualGridEventArgs e)
		{
			if (e.ViewInfo.SortDescriptors.Count == 0)
				return;

			var propertyName = e.ViewInfo.SortDescriptors[0].PropertyName;

			if (propertyName == "OrderStatus")
				return;
		}

		private void GridControl_CellFormatting(Object sender, VirtualGridCellElementEventArgs e)
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
								if (e.CellElement.Value is System.Drawing.Image image)
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

		private void GridControl_QueryHasChildRows(Object sender, VirtualGridQueryHasChildRowsEventArgs e)
		{
			e.HasChildRows = e.ViewInfo == this.GridControl.MasterViewInfo;
		}

		private void GridControl_RowExpanding(Object sender, VirtualGridRowExpandingEventArgs e)
		{
			this.GridControl.TableElement.SynchronizeRows();
		}

		private void GridControl_ScreenTipNeeded(Object sender, ScreenTipNeededEventArgs e)
		{
			if (e.Item is VirtualGridCellElement cell && cell.ViewInfo != GridControl.MasterViewInfo)
			{
				var cellIndex = cell.RowIndex;
				if (cell != null && cellIndex >= 0)
				{
					if (cell.ColumnIndex == 0)
					{
						var inventory = MainRepository.InventoriesCache
										.Where(p => p.ProductID == detailsData[cellIndex].ProductID)
										.SingleOrDefault();
						ShowScreenTipForCell(cell, cellIndex, inventory);
					}
				}
			}
		}

		private async void AcceptButton_Click(Object sender, EventArgs e)
		{
			if (Properties.Settings.Default.AskConfirmation)
			{
				var dialogResult = RadMessageBox.Show(
													MESSAGE.Gui.CONFIRM_ACTION,
													"<html><b>Подтверждение</b>",
													MessageBoxButtons.OKCancel
													);

				if (dialogResult == DialogResult.Cancel)
					return;
			}

			var cart = data[0];

			if (!WebWorker.HasConnection)
			{
				RadMessageBox.Show(
								MESSAGE.Gui.ON_CONNECT_ERROR_TRANSACTION,
								"<html><b>Подключение</b>"
								);
				cart.Status = (byte)ShoppingCartStatus.Pending;
				Entity.Commit();
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
											MESSAGE.Gui.ON_SUCCESS_TRANSACTION,
											caption: "Успех!",
											buttons: MessageBoxButtons.OK
											)
				);

				SendMail(transactionHistory);
			});
			RemoveCartCallback();
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

		#region Fields

		private RadButton radAcceptButton;

		private List<ShoppingCart> data = new List<ShoppingCart>();

		private List<ShoppingCartItem> detailsData = new List<ShoppingCartItem>();

		private readonly List<GridElement> cellElements = new List<GridElement>();

		#endregion
	}
}
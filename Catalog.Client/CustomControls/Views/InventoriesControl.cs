using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;

using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;

using Catalog.Common.Utils;
using Catalog.Common.Service;
using Catalog.Common.Repository;
using Catalog.Client.Properties;


namespace Catalog.Client
{
	using FilterPredicate = System.Linq.Expressions.Expression<Func<ProductInventory, bool>>;

	class InventoriesControl : BaseGridControl
	{
		#region Properties

		public bool IsPriceFilterApplied { get; set; }

		public decimal MinPrice { get; private set; }

		public decimal MaxPrice { get; private set; }

		public String StatusText { get; private set; }

		private Action<DbQuery<ProductInventory>> Callback { get; set; }

		#endregion

		#region Initialisation

		public InventoriesControl(ProductTag tag)
			: base(tag)
		{
			this.GridControl.MasterViewInfo.MinColumnWidth = 100;
			IsPriceFilterApplied = false;
			MinPrice = 0.00M;
			MaxPrice = Decimal.MaxValue;
		}

		protected override void Initialize()
		{
			columnTypes = new Type[] {
				typeof(Image),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(decimal),
				typeof(int),
			};
			this.columnNames.Add("Фото");
			this.columnNames.Add("Артикул");
			this.columnNames.Add("Наименование товара");
			this.columnNames.Add("НН");
			this.columnNames.Add("МСК");
			this.columnNames.Add("Цена");
			this.columnNames.Add("Количество");

			this.GridControl.ColumnCount = columnNames.Count;
			this.GridControl.RowCount = (Tag as ProductTag).Count;

			this.GridControl.MasterViewInfo.SetColumnDataType(columnTypes);
			this.GridControl.MasterViewInfo.RegisterCustomColumn(0);
			this.GridControl.MasterViewInfo.RegisterCustomColumn(6);

			this.Callback = query =>
			{
				try
				{
					this.data = query.ToList();
					this.GridControl.RowCount = data.Count;
					cellElements.ForEach(el => (el as QuantityCellElement).ProductInventories = data);
					this.GridControl.TableElement.SynchronizeRows();
					this.GridControl.MasterViewInfo.MinColumnWidth = 100;
				}
				catch (Exception e)
				{
					Debug.WriteLine($"{e.Message}\n{e.StackTrace}");
				}

				MainForm.StatusLabelElement.Text = String.Format(StatusText, GridControl.RowCount);
				this.DataLoaded = true;
				this.GridControl.AutoSizeColumnsMode = VirtualGridAutoSizeColumnsMode.Fill;
				this.GridControl.MasterViewInfo.IsWaiting = false;
				TopButton.Enabled = true;
			};

			GridControl.CellFormatting += GridControl_CellFormatting;
		}

		#endregion

		#region Data handling

		public override void RefreshData()
		{
			MainForm.StatusLabelElement.Text = "Обновление товаров из базы данных";
			StatusText = "Обновление товаров завершено! Обновлено {0} товаров.";

			var productTag = Tag as ProductTag;
			FilterPredicate pred;

			if (productTag.IsAbsolute)
				return;

			if (productTag.IsZero)
				pred = null;
			else if (productTag.SID == -1)
				pred = p => productTag.CID == p.ProductCategoryID;
			else
				pred = p => productTag.SID == p.ProductSubcategoryID
						 && productTag.CID == p.ProductCategoryID;

			FilterDataAsync(pred);
		}

		protected void FilterDataAsync(FilterPredicate predicate)
		{
			GridControl.MasterViewInfo.IsWaiting = true;
			JobsFinised = false;
			data.Clear();
			DataLoaded = false;
			TopButton.Enabled = false;

			var task = Task.Run(() =>
			{
				DbQuery<ProductInventory> inventories = null;
				if (predicate != null)
					inventories = Repository.Context.ProductInventories
													.Where(predicate)
													as DbQuery<ProductInventory>;
				else
					inventories = Repository.Context.ProductInventories;

				var query = inventories;
				query = SortHelper.ProductInventorySort(query, sortMode, GridControl.SortDescriptors);

				return query;
			});

			ExecuteQueryAsync(task, Callback);
		}

		public void ResetFilter()
		{
			if (IsPriceFilterApplied)
			{
				MinPrice = 0.00M;
				MaxPrice = Decimal.MaxValue;
				radToTextBox.Text = String.Empty;
				radFromTextBox.Text = String.Empty;
				IsPriceFilterApplied = false;
				RefreshData();
			}
		}

		#endregion

		#region Event handling

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.radToTextBox.TextChanging += TextBox_TextChanging;
			this.radFromTextBox.TextChanging += TextBox_TextChanging;
			GridControl.ScreenTipNeeded += GridControl_ScreenTipNeeded;
			SearchTextBox.Search += SearchBox_Search;
			SearchTextBox.Clean += SearchBox_Reset;
		}

		protected void TextBox_TextChanging(Object sender, TextChangingEventArgs e)
		{
			var name = ((RadTextBox)sender).Name;

			if (e.NewValue.Length > 0)
			{
				if (name == "radToTextBox")
				{
					Decimal.TryParse(e.NewValue, out var maxN);
					MaxPrice = maxN;
				}
				else if (name == "radFromTextBox")
				{
					Decimal.TryParse(e.NewValue, out var minN);
					MinPrice = minN;
				}
				var productTag = Tag as ProductTag;

				FilterPredicate pred;
				if (productTag.CID == 0)
					pred = (p => p.Product.Price >= MinPrice && p.Product.Price <= MaxPrice);
				else if (productTag.SID == -1)
				{
					pred = (p => (p.Product.Price >= MinPrice && p.Product.Price <= MaxPrice) &&
									p.Product.ProductSubcategory.ProductCategoryID == productTag.CID);
				}
				else
				{
					pred = (p => (p.Product.Price >= MinPrice && p.Product.Price <= MaxPrice) &&
								(p.Product.ProductSubcategoryID == productTag.SID &&
								 p.Product.ProductSubcategory.ProductCategoryID == productTag.CID));
				}
				IsPriceFilterApplied = true;
				FilterDataAsync(pred);
			}
		}

		protected virtual void SearchBox_Search(object sender, SearchTextBox.SearchBoxEventArgs e)
		{
			var term = e.SearchText;

			this.StatusText = "Поиск товаров завершен! Найдено {0} товаров.";

			if (MinPrice != 0M || MaxPrice != 0M)
			{
				FilterDataAsync(p => (p.Product.Name.ToLower().Contains(term) ||
								p.Product.Code.ToLower().Contains(term) ||
								p.Product.Brand.ToLower().Contains(term))
								&& (p.Product.Price >= MinPrice && p.Product.Price <= MaxPrice));
			}
			else FilterDataAsync(
				  p => p.Product.Name.ToLower().Contains(term) ||
					   p.Product.Code.ToLower().Contains(term) ||
					   p.Product.Brand.ToLower().Contains(term));


		}

		protected virtual void SearchBox_Reset(object sender, EventArgs te)
		{
			ResetView();
			RefreshData();
		}

		protected override void GridControl_CreateCellElement(object sender, VirtualGridCreateCellEventArgs e)
		{
			if (e.RowIndex >= 0)
				switch (e.ColumnIndex)
				{
					case 0:
						e.CellElement = new ProductPhotoCellElement();
						break;
					case 6:
						e.CellElement = new QuantityCellElement(ProductInventories, e.ColumnIndex)
						{ ProductInventories = data };
						this.cellElements.Add(e.CellElement);
						break;
				}
		}

		protected override void GridControl_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
		{
			if (e.ColumnIndex < 0)
				return;

			base.GridControl_CellValueNeeded(sender, e);

			if (e.RowIndex >= 0)
			{
				if (e.RowIndex < data.Count && data.Count > 0)
				{
					var rowData = data[e.RowIndex];
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
							e.Value = rowData.Product.ArticleNumber;
							break;
						case 2:
							e.Value = rowData.Product.Name;
							break;
						case 3:
							e.Value = rowData.StockLevel1;
							e.FormatString = "<html><b>{0}";
							break;
						case 4:
							e.Value = rowData.StockLevel2;
							e.FormatString = "<html><b>{0}";
							break;
						case 5:
							var price = rowData.Product.Price;
							e.Value = price;
							e.FormatString = CultureConfig.CurrencyInfo.Format(price);
							break;
						case 6:
							var qnty = ProductInventories.Get(rowData.ProductID);
							e.Value = qnty != null ? qnty.Quantity : 0;
							break;
					}
				}
			}
			if (e.RowIndex < 0)
			{
				e.Value = columnNames[e.ColumnIndex];
				e.FormatString = "<html><b>{0}";

				if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
				{
					e.FieldName = FieldsHelper.InventoriesFields[e.ColumnIndex];
				}
			}

		}

		protected void GridControl_CellFormatting(object sender, VirtualGridCellElementEventArgs e)
		{
			if (e.CellElement is VirtualGridHeaderCellElement)
			{
				var cell = e.CellElement as VirtualGridHeaderCellElement;
				cell.TextWrap = true;

				switch (cell.ColumnIndex)
				{
					case 0:
					case 6:
						cell.Arrow.ShouldPaint = false;
						break;
					default:
						cell.Arrow.ResetValue(RadItem.ShouldPaintProperty, ValueResetFlags.Local);
						break;
				}

			}

			if (e.CellElement.RowIndex >= 0)
			{
				e.CellElement.TextWrap = true;

				switch (e.CellElement.ColumnIndex)
				{
					case 0: // ProductPhoto
						{
							if (e.CellElement.Value is Image)
							{
								e.CellElement.Image = (Image)e.CellElement.Value;
								e.CellElement.Text = String.Empty;
								e.ViewInfo.SetColumnWidth(e.CellElement.ColumnIndex, 60);
							}
						}
						break;
					case 2:
						{
							e.CellElement.TextWrap = true;
							e.CellElement.TextAlignment = ContentAlignment.MiddleLeft;
							e.CellElement.Image = null;
							e.ViewInfo.SetColumnWidth(e.CellElement.ColumnIndex, GridControl.Width - ((columnNames.Count - 1) * 120) - 20);

						}
						break;
					default:
						if (e.CellElement.Text.Length > 0)
							e.CellElement.ToolTipText = e.CellElement.Value.ToString();
						e.ViewInfo.SetColumnWidth(e.CellElement.ColumnIndex, 120);
						e.CellElement.Image = null;
						break;
				}
			}
		}

		protected void GridControl_ScreenTipNeeded(Object sender, ScreenTipNeededEventArgs e)
		{
			if (e.Item is VirtualGridCellElement cell)
			{
				var cellIndex = cell.RowIndex;
				if (cell != null && cellIndex >= 0)
				{
					if (cell.ColumnIndex == 0)
						ShowScreenTipForCell(cell, cellIndex, data[cellIndex]);
				}
			}
		}

		private void GridControl_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
		{
			if (sender is GridDataCellElement cell && cell.ColumnInfo.Name == "Name")
			{
				e.ToolTipText = cell.Value.ToString();
			}
		}

		protected override void GridControl_SortChanged(object sender, VirtualGridEventArgs e)
		{
			if (e.ViewInfo.SortDescriptors.Count == 0)
				return;

			var propertyName = e.ViewInfo.SortDescriptors[0].PropertyName;

			if (propertyName == "Photo" || propertyName == "Quantity")
				return;

			if (propertyName == "ArticleNumber")
				sortMode = ProductInventorySortMode.ArticleNumber;
			else if (propertyName == "Price")
				sortMode = ProductInventorySortMode.Price;
			else if (propertyName == "Name")
				sortMode = ProductInventorySortMode.Name;
			else
				sortMode = ProductInventorySortMode.None;

			base.GridControl_SortChanged(sender, e);
		}

		#endregion

		#region Export and ScreenTip handling

		protected override Workbook CreateWorkbook()
		{
			var workbook = new Workbook();
			Worksheet worksheet = workbook.Worksheets.Add();

			void alignCell(CellSelection cellSelection)
			{
				cellSelection.SetVerticalAlignment(
					Telerik.Windows.Documents.Spreadsheet.Model.RadVerticalAlignment.Center
				);
				cellSelection.SetHorizontalAlignment(
					Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Center
				);
			}

			var names = columnNames;
			names.Insert(2, "Код");
			names.Insert(3, "Бренд");
			names.RemoveAt(0);
			names.RemoveAt(columnNames.Count - 1);
			var tag = this.Tag as ProductTag;
			var categoryName = MainRepository.ProductCategoriesCache
											.Where(c => true && c.ProductCategoryID == tag.CID)
											.SingleOrDefault()?
											.Name;

			var subcategoryName = MainRepository.ProductSubcategoriesCache
												.Where(c => true && c.ProductSubcategoryID == tag.SID)
												.SingleOrDefault()?
												.Name;

			categoryName = categoryName is null ? "" : categoryName;
			subcategoryName = categoryName is null ? "" : subcategoryName;
			// set header
			for (int i = 0; i < names.Count; i++)
			{
				CellSelection selection = worksheet.Cells[0, i];
				selection.SetValue(names[i]);
				alignCell(selection);
			}

			for (int i = 0; i < data.Count - 1; i++)
			{
				int rowIndex = i + 1;
				CellSelection selection = worksheet.Cells[rowIndex, 0];
				selection.SetValue(data[i].Product.ArticleNumber);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 1];
				selection.SetValue(data[i].Product.Code);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 2];
				selection.SetValue(data[i].Product.Brand);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 3];
				selection.SetValue(data[i].Product.Name);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 4];
				selection.SetValue(data[i].StockLevel1);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 5];
				selection.SetValue(data[i].StockLevel2);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 6];
				selection.SetValue(Convert.ToDouble(data[i].Product.Price));
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 7];
				selection.SetValue(categoryName);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 8];
				selection.SetValue(subcategoryName);
				alignCell(selection);
			}

			for (var i = 0; i < 2; ++i)
			{
				CellSelection selection = worksheet.Cells[0, names.Count];
				selection.SetValue("Категория");
				alignCell(selection);

				if (subcategoryName.Length > 0)
				{
					selection = worksheet.Cells[0, names.Count + 1];
					selection.SetValue("Подкатегория");
					alignCell(selection);
				}
			}

			worksheet.Columns[worksheet.UsedCellRange].AutoFitWidth();
			worksheet.Name = "Инвентарь";
			return workbook;
		}

		#endregion

		#region Fields

		private List<ProductInventory> data = new List<ProductInventory>();

		private ProductInventorySortMode sortMode = ProductInventorySortMode.None;

		private List<VirtualGridCellElement> cellElements = new List<VirtualGridCellElement>();

		#endregion
	}
}

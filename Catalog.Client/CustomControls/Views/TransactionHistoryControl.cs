using System;
using System.Linq;
using System.Collections.Generic;

using Catalog.Common.Repository;
using Catalog.Common.Service;
using Catalog.Common.Utils;

using Telerik.WinControls.UI;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Catalog.Client
{
	class TransactionHistoryControl : BaseGridControl
	{
		internal class Comparer : IComparer<TransactionHistory>
		{
			public Int32 Compare(TransactionHistory x, TransactionHistory y)
			{
				return y.TransactionDate.CompareTo(x.TransactionDate);
			}
		}

		#region Properties

		List<TransactionHistory> data = new List<TransactionHistory>();

		Action<List<TransactionHistory>> QueryCallback { get; set; }

		#endregion

		#region Initializers

		public TransactionHistoryControl() : base(null)
		{
			this.radToTextBox.Visible = false;
			this.radFromTextBox.Visible = false;

			var ctrl = this.radTableLayoutPanel.GetControlFromPosition(0, 0);
			ctrl.Visible = false;
			this.GridControl.AllowMultiColumnSorting = true;
		}

		protected override void Initialize()
		{
			this.exportFileName = "История.xlsx";
			columnTypes = new Type[]
			{
				typeof(DateTime),
				typeof(int),
				typeof(int),
				typeof(decimal)
			};

			this.columnNames.Add("Дата и время заказа");
			this.columnNames.Add("Номер заказа");
			this.columnNames.Add("Товары");
			this.columnNames.Add("Cумма");
			this.GridControl.ColumnCount = columnNames.Count;
			this.GridControl.MasterViewInfo.SetColumnDataType(columnTypes);

			this.QueryCallback = query =>
			{
				this.data = query.ToList();
				this.GridControl.RowCount = this.data.Count;

				if (data.Count > 0)
				{
					GridControl.ColumnCount = columnNames.Count;
					DataLoaded = true;
					this.GridControl.TableElement.SynchronizeRows();
				}
				else GridControl.ColumnCount = 0;

				this.ExportButton.Enabled = DataLoaded;
				this.GridControl.MasterViewInfo.AllowColumnResize = false;
				this.GridControl.MasterViewInfo.IsWaiting = false;
			};

		}

		private void OnLoad(Object sender, EventArgs e)
		{
			base.OnLoad(e);
			RefreshData();
		}

		#endregion

		#region Data handlers

		public override void RefreshData()
		{
			data.Clear();
			FilterDataAsync();
		}

		private void FilterDataAsync()
		{
			this.GridControl.MasterViewInfo.IsWaiting = true;
			this.DataLoaded = false;

			var query = Repository.Context.TransactionHistories;
			ExecuteQuery(query.ToList(), QueryCallback);
		}

		#endregion

		#region Event handlers

		protected override void GridControl_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
		{
			base.GridControl_CellValueNeeded(sender, e);

			if (e.ColumnIndex < 0 || data.Count == 0)
				return;

			if (e.RowIndex == RadVirtualGrid.HeaderRowIndex)
			{
				e.Value = columnNames[e.ColumnIndex];
				e.FormatString = "<html><b>{0}";
				e.FieldName = FieldsHelper.TransactionHistoryFields[e.ColumnIndex];
			}
			if (e.RowIndex >= 0 && data.Count > 0)
			{
				var rowData = data[e.RowIndex];
				switch (e.ColumnIndex)
				{
					case 0:
						e.Value = rowData.TransactionDate;
						e.FormatString = CultureConfig.DateTimeInfo.Format(rowData.TransactionDate);
						break;
					case 1:
						e.Value = rowData.TransactionID;
						break;
					case 2:
						e.Value = rowData.Quantity;
						break;
					case 3:
						e.Value = rowData.TotalCost;
						e.FormatString = CultureConfig.CurrencyInfo.Format(rowData.TotalCost);
						break;
				}
			}
		}

		protected void GridControl_CellFormatting(object sender, VirtualGridCellElementEventArgs e)
		{
			if (e.CellElement is VirtualGridHeaderCellElement)
			{
				e.CellElement.TextWrap = true;
			}
			if (e.CellElement is VirtualGridFilterCellElement)
			{
				e.CellElement.Enabled = false;
			}
			if (e.CellElement.RowIndex >= 0)
			{
				e.CellElement.TextWrap = true;
				e.CellElement.Image = null;
			}
		}

		protected override void GridControl_SortChanged(object sender, VirtualGridEventArgs e)
		{
			if (e.ViewInfo.SortDescriptors.Count == 0)
			{
				return;
			}

			var propertyName = e.ViewInfo.SortDescriptors[0].PropertyName;
			if (propertyName == "OrderStatus")
			{
				return;
			}
			var prop = typeof(TransactionHistory).GetProperty(propertyName).PropertyType;
			if (prop.IsValueType || prop == typeof(string))
			{
				base.GridControl_SortChanged(sender, e);
			}

		}

		#endregion

		#region Export and ScreenTip handlers

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

			// set header
			for (int i = 0; i < columnNames.Count; i++)
			{
				CellSelection selection = worksheet.Cells[0, i];
				selection.SetValue(columnNames[i]);
			}

			for (int i = 0; i < data.Count; i++)
			{
				TransactionHistory history = data[i];
				int rowIndex = i + 1;
				CellSelection selection = worksheet.Cells[rowIndex, 0];
				selection.SetValue(history.TransactionDate);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 1];
				selection.SetValue(history.TransactionID);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 2];
				selection.SetValue(history.Quantity);
				alignCell(selection);

				selection = worksheet.Cells[rowIndex, 3];
				selection.SetValue(history.TotalCost.ToString());
				selection.SetFormat(new CellValueFormat("## ###.## ₽"));
				alignCell(selection);
			}

			worksheet.Columns[worksheet.UsedCellRange].AutoFitWidth();
			worksheet.Name = "Корзина";
			return workbook;
		}

		#endregion
	}
}
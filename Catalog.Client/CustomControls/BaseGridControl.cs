﻿using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;

using Catalog.Common;
using Catalog.Common.Repository;
using Catalog.Common.Utils;
using Catalog.Client.Properties;

namespace Catalog.Client
{
	public partial class BaseGridControl : UserControl
	{
		#region Properties

		public bool JobsFinised { get; set; }

		public bool DataLoaded { get; protected set; } = false;

		public static InventoryCollection ProductInventories { get; set; } = new InventoryCollection();

		public RadVirtualGrid GridControl
		{
			get { return radVirtualGrid; }
		}

		public static SearchTextBox SearchTextBox { set; get; }

		public static RadButton TopButton { get; set; }

		public RadButton ExportButton { get => radExportButton; }

		#endregion

		public BaseGridControl(ProductTag tag)
		{
			InitializeComponent();

			Tag = tag;
			this.exportFileName = "Товары.xlsx";
			this.columnNames = new List<string>();
			this.detailColumnNames = new List<string>();
			this.Dock = DockStyle.Fill;
			this.Margin = new Padding(0);
			this.Padding = new Padding(0);
			this.GridControl.TableElement.Alignment = ContentAlignment.MiddleLeft;
			this.GridControl.VirtualGridElement.TextAlignment = ContentAlignment.MiddleLeft;
			this.GridControl.MasterViewInfo.HeaderRowHeight = 40;
			this.GridControl.MasterViewInfo.MinRowHeight = 50;
			this.GridControl.MasterViewInfo.AllowColumnResize = false;
			this.GridControl.MasterViewInfo.AllowRowResize = false;
			this.GridControl.MasterViewInfo.AllowEdit = false;
			Initialize();
			this.GridControl.CreateCellElement += GridControl_CreateCellElement;
			this.GridControl.SortChanged += GridControl_SortChanged;
			this.GridControl.CellValueNeeded += GridControl_CellValueNeeded;

			void RadTextBox_KeyPress(Object sender, KeyPressEventArgs e)
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
					e.Handled = true;
			};

			void RadTextBox_Leave(Object sender, EventArgs e)
			{
				if (radFromTextBox.Text.Length == 0)
					RefreshData();
				if (radToTextBox.Text.Length == 0)
					RefreshData();
			}

			this.radFromTextBox.KeyPress += RadTextBox_KeyPress;
			this.radToTextBox.KeyPress += RadTextBox_KeyPress;

			this.radFromTextBox.Leave += RadTextBox_Leave;
			this.radToTextBox.Leave += RadTextBox_Leave;
		}

		protected void ShowScreenTipForCell(VirtualGridCellElement cell, int cellIndex, Common.Service.ProductInventory inventory)
		{
			using (var defaultFont = new Font("Segoe UI", 14F))
			{
				var screenTip = new RadOffice2007ScreenTipElement()
				{
					EnableCustomSize = true
				};

				screenTip.CaptionLabel.Margin = new Padding(3);
				screenTip.CaptionLabel.Text = $"<html><b>Бренд</b>: {inventory?.Product.Brand}\n" +
											  $"<b>Код</b>:         {inventory?.Product.Code}\n" +
											  $"<b>Упаковка</b>:    {inventory.Pack}";
				screenTip.CaptionLabel.TextWrap = true;
				screenTip.CaptionLabel.CustomFontSize = 12F;

				var imgData = MainRepository.ProductPhotoCache
											.Where(p => p.ProductID == inventory.ProductID)
											.FirstOrDefault();

				if (imgData == null || imgData.LargePhoto == null)
				{
					screenTip.MainTextLabel.Image = Resources.placeholder;
					screenTip.MaxSize = new Size(400, 400);
				}
				else
				{
					Image tipImage = ImageUtils.GetImage(imgData.LargePhoto);

					int width = tipImage.Width,
						height = Int32.MaxValue;
					screenTip.MaxSize = new Size(width, height);
					screenTip.MainTextLabel.Image = tipImage;
				}

				screenTip.MainTextLabel.Text = String.Empty;
				screenTip.MainTextLabel.ImageAlignment = ContentAlignment.MiddleCenter;

				var description = inventory?.Product.Description;

				if (description != null && description.Length > 0)
				{
					var text = $"<html><b>Описание:</b> {description}";
					screenTip.FooterTextLabel.Text = text;
					screenTip.FooterTextLabel.Font = defaultFont;
					screenTip.FooterTextLabel.CustomFontSize = 12F;
					screenTip.FooterTextLabel.TextWrap = true;
					screenTip.FooterVisible = true;
				}
				screenTip.EnableCustomSize = false;
				cell.ScreenTip = screenTip;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			RefreshData();
		}

		protected async void ExecuteQueryAsync<T>(Task<T> task, Action<T> callback)
		{
			if (callback == null)
				return;
			var result = await task;
			callback(result);
		}

		protected void ExecuteQuery<T>(T task, Action<T> callback)
		{
			if (callback == null || task == null)
				return;
			callback(task);
		}

		#region Virtual methods

		protected virtual void Initialize()
		{ }

		public virtual void ResetView(bool fitColumns = true)
		{
		}

		public virtual void RefreshData()
		{
		}

		#endregion

		#region Event handling

		protected virtual void GridControl_SortChanged(object sender, VirtualGridEventArgs e)
		{
			RefreshData();
		}

		protected virtual void GridControl_CreateCellElement(object sender, VirtualGridCreateCellEventArgs e)
		{ }

		protected virtual void GridControl_CellValueNeeded(object sender, VirtualGridCellValueNeededEventArgs e)
		{ }

		#endregion

		#region Export handling

		protected virtual void ExportButton_Click(object sender, EventArgs e)
		{
			if (GridControl.MasterViewInfo.IsWaiting)
			{
				RadMessageBox.Show("The Data is not loaded! Please wait!");
				return;
			}

			using (var savefile = new SaveFileDialog
			{
				FileName = exportFileName,
				Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*"
			})
			{
				if (savefile.ShowDialog() == DialogResult.OK)
				{
					var formatProvider = new XlsxFormatProvider();

					using (Stream output = new FileStream(savefile.FileName, FileMode.Create))
					{
						try
						{
							formatProvider.Export(CreateWorkbook(), output);
						}
						catch (Exception ex)
						{
							RadMessageBox.Show($"Закройте файл и повтрите\n{ex.Message}\n{ex.StackTrace}", "Экспорт", MessageBoxButtons.AbortRetryIgnore);
						}
					}
				}
			}
		}

		protected virtual Workbook CreateWorkbook()
		{
			return new Workbook();
		}

		#endregion

		#region Fields

		protected List<String> columnNames;

		protected List<String> detailColumnNames;

		protected String exportFileName;

		protected ISavableObject currentItem;

		protected Type[] columnTypes;

		protected string dataFormText;

		#endregion
	}

}

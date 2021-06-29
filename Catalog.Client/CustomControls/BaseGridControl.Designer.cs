using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    partial class BaseGridControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.radVirtualGrid = new Telerik.WinControls.UI.RadVirtualGrid();
			this.radTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.radFromTextBox = new Telerik.WinControls.UI.RadTextBox();
			this.radToTextBox = new Telerik.WinControls.UI.RadTextBox();
			this.radExportButton = new Telerik.WinControls.UI.RadButton();
			this.customShape1 = new Telerik.WinControls.OldShapeEditor.CustomShape();
			this.customShape2 = new Telerik.WinControls.CustomShape(this.components);
			this.customShape3 = new Telerik.WinControls.CustomShape(this.components);
			((System.ComponentModel.ISupportInitialize)(this.radVirtualGrid)).BeginInit();
			this.radTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.radFromTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radToTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radExportButton)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView
			// 
			this.radVirtualGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radVirtualGrid.EnableGestures = false;
			this.radVirtualGrid.Location = new System.Drawing.Point(3, 48);
			this.radVirtualGrid.MasterViewInfo.AutoSizeColumnsMode = Telerik.WinControls.UI.VirtualGridAutoSizeColumnsMode.Fill;
			this.radVirtualGrid.MasterViewInfo.ShowFilterRow = false;
			this.radVirtualGrid.MasterViewInfo.ShowNewRow = false;
			this.radVirtualGrid.Name = "dataGridView";
			this.radVirtualGrid.SelectionMode = Telerik.WinControls.UI.VirtualGridSelectionMode.FullRowSelect;
			this.radVirtualGrid.Size = new System.Drawing.Size(778, 511);
			this.radVirtualGrid.TabIndex = 0;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).ShowHorizontalLine = false;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).TextWrap = true;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).TextOrientation = System.Windows.Forms.Orientation.Horizontal;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).CustomFont = "None";
			//((Telerik.WinControls.UI.RadVirtualGridElement)(this.dataGridView.GetChildAt(0))).CustomFontSize = 12F;
			((Telerik.WinControls.UI.RadVirtualGridElement)(this.radVirtualGrid.GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).FilterRowHeight = 40;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).RowHeight = 50;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).ShowHorizontalLine = false;
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).Text = "Товары отсутсвуют";
			((Telerik.WinControls.UI.VirtualGridTableElement)(this.radVirtualGrid.GetChildAt(0).GetChildAt(0))).CustomFont = "TelerikWebUI";
			//((Telerik.WinControls.UI.VirtualGridTableElement)(this.dataGridView.GetChildAt(0).GetChildAt(0))).CustomFontSize = 12F;


			var filterLayoutControl = new RadLayoutControl();
			filterLayoutControl.Controls.Add(this.radFromTextBox);
			filterLayoutControl.Controls.Add(this.radToTextBox);
			filterLayoutControl.Dock = DockStyle.Fill;
			filterLayoutControl.Margin = new System.Windows.Forms.Padding(0);
			// 
			// 
			//
			var roundRectShape1 = new RoundRectShape();
			roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);

			// 
			// tableLayoutPanel
			// 
			this.radTableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
			this.radTableLayoutPanel.ColumnCount = 4;
			this.radTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
			this.radTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.radTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
			this.radTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
			
			this.radTableLayoutPanel.Controls.Add(filterLayoutControl, 0, 0);
			this.radTableLayoutPanel.Controls.Add(this.radVirtualGrid, 0, 1);
			this.radTableLayoutPanel.Controls.Add(this.radExportButton, 3, 0);
			this.radTableLayoutPanel.SetColumnSpan(this.radVirtualGrid, 4);

			this.radTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.radTableLayoutPanel.Name = "tableLayoutPanel";
			this.radTableLayoutPanel.RowCount = 2;
			this.radTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.radTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.radTableLayoutPanel.Size = new System.Drawing.Size(784, 562);
			this.radTableLayoutPanel.TabIndex = 1;
			// 
			// radFromTextBox
			// 
			this.radFromTextBox.AutoSize = true;
			this.radFromTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radFromTextBox.Location = new System.Drawing.Point(0, 0);
			this.radFromTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.radFromTextBox.Name = "radFromTextBox";
			this.radFromTextBox.NullText = "От";
			this.radFromTextBox.Size = new System.Drawing.Size(120, 45);
			this.radFromTextBox.TabIndex = 5;
			this.radFromTextBox.Tag = "0";
			this.radFromTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// radToTextBox
			// 
			this.radToTextBox.AutoSize = true;
			this.radToTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radToTextBox.Location = new System.Drawing.Point(120, 0);
			this.radToTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.radToTextBox.Name = "radToTextBox";
			this.radToTextBox.NullText = "До";
			this.radToTextBox.Size = new System.Drawing.Size(120, 45);
			this.radToTextBox.TabIndex = 6;
			this.radToTextBox.Tag = "1";
			this.radToTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// exportButton
			// 
			this.radExportButton.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.radExportButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radExportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.radExportButton.Image = global::Catalog.Client.Properties.Resources.ms_excel;
			this.radExportButton.Location = new System.Drawing.Point(547, 3);
			this.radExportButton.Name = "exportButton";
			this.radExportButton.Size = new System.Drawing.Size(114, 39);
			this.radExportButton.TabIndex = 3;
			this.radExportButton.Text = "Excel";
			this.radExportButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.radExportButton.Click += new System.EventHandler(this.ExportButton_Click);
			// 
			// customShape1
			// 
			this.customShape1.Dimension = new System.Drawing.Rectangle(0, 0, 0, 0);
			// 
			// customShape2
			// 
			this.customShape2.AsString = "20,20,200,100:20,20,False,0,0,0,0,0:220,20,False,0,0,0,0,0:220,120,False,0,0,0,0," +
    "0:20,120,False,0,0,0,0,0:";
			// 
			// customShape3
			// 
			this.customShape3.AsString = "20,20,200,100:20,20,False,0,0,0,0,0:220,20,False,0,0,0,0,0:220,120,False,0,0,0,0," +
    "0:20,120,False,0,0,0,0,0:";
			// 
			// BaseGridControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.radTableLayoutPanel);
			this.Name = "BaseGridControl";
			this.Size = new System.Drawing.Size(784, 562);
			((System.ComponentModel.ISupportInitialize)(this.radVirtualGrid)).EndInit();
			this.radTableLayoutPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.radFromTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radToTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radExportButton)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private RadVirtualGrid radVirtualGrid;
		protected RadButton radExportButton;
        private Telerik.WinControls.OldShapeEditor.CustomShape customShape1;
        private CustomShape customShape2;
        private CustomShape customShape3;
		protected RadTextBox radFromTextBox;
		protected RadTextBox radToTextBox;
		protected RadTextBoxElement radFromTextBoxElement;
		protected RadTextBoxElement radToTextBoxElement;
		protected TableLayoutPanel radTableLayoutPanel;
	}
}

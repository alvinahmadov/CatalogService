
using System;

namespace Catalog.Client
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
			this.topControl = new TopControl(this);
			this.radCollapsiblePanel = new Telerik.WinControls.UI.RadCollapsiblePanel();
			this.radTreeView = new Telerik.WinControls.UI.RadTreeView();
			this.tabEdgeShape = new Telerik.WinControls.UI.TabEdgeShape();
			this.radBreadCrumb = new Telerik.WinControls.UI.RadBreadCrumb();
			this.statusControl = new StatusControl();
			this.radStatusStrip = new Telerik.WinControls.UI.RadStatusStrip();
			StatusLabelElement = new Telerik.WinControls.UI.RadLabelElement();
			this.tableLayoutPanelTop.SuspendLayout();
			this.tableLayoutPanelBottom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel)).BeginInit();
			this.radCollapsiblePanel.PanelContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.radTreeView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radBreadCrumb)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radStatusStrip)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanelTop
			// 
			this.tableLayoutPanelTop.BackColor = System.Drawing.SystemColors.Window;
			this.tableLayoutPanelTop.ColumnCount = 1;
			this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelTop.Controls.Add(this.topControl,				0, 0);
			this.tableLayoutPanelTop.Controls.Add(this.tableLayoutPanelBottom,	0, 1);
			this.tableLayoutPanelTop.Controls.Add(this.statusControl,			0, 2);
			this.tableLayoutPanelTop.Controls.Add(this.radStatusStrip,			0, 3);
			this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
			this.tableLayoutPanelTop.RowCount = 4;
			this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanelTop.Size = new System.Drawing.Size(1276, 880);
			this.tableLayoutPanelTop.TabIndex = 1;
			// 
			// topControl
			// 
			this.topControl.BackColor = System.Drawing.SystemColors.Window;
			this.topControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.topControl.Location = new System.Drawing.Point(0, 30);
			this.topControl.Margin = new System.Windows.Forms.Padding(0);
			this.topControl.Name = "topControl";
			this.topControl.Size = new System.Drawing.Size(1276, 70);
			this.topControl.TabIndex = 0;
			// 
			// tableLayoutPanelBottom
			// 
			this.tableLayoutPanelBottom.Controls.Add(this.radCollapsiblePanel, 0, 0);
			this.tableLayoutPanelBottom.Controls.Add(this.radBreadCrumb, 1, 0);
			this.tableLayoutPanelBottom.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanelBottom.ColumnCount = 2;
			this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
			this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelBottom.RowCount = 2;
			this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

			this.tableLayoutPanelBottom.SetRowSpan(this.radCollapsiblePanel, 2);
			this.tableLayoutPanelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelBottom.Location = new System.Drawing.Point(0, 150);
			this.tableLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
			this.tableLayoutPanelBottom.Size = new System.Drawing.Size(1276, 660);
			this.tableLayoutPanelBottom.TabIndex = 1;
			
			//// 
			//// radLeftCollapsiblePanel
			//// 
			this.radCollapsiblePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radCollapsiblePanel.AnimationType = Telerik.WinControls.UI.CollapsiblePanelAnimationType.Slide;
			this.radCollapsiblePanel.EnableAnimation = false;
			this.radCollapsiblePanel.ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
			this.radCollapsiblePanel.HorizontalHeaderAlignment = Telerik.WinControls.UI.RadHorizontalAlignment.Center;
			this.radCollapsiblePanel.Location = new System.Drawing.Point(0, 0);
			this.radCollapsiblePanel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.radCollapsiblePanel.Size = new System.Drawing.Size(collapsiblePanelMaxWidth, 600);
			this.radCollapsiblePanel.Name = "radLeftCollapsiblePanel";
			this.radCollapsiblePanel.PanelContainer.Controls.Add(this.radTreeView);
			this.radCollapsiblePanel.PanelContainer.Location = new System.Drawing.Point(0, 0);
			this.radCollapsiblePanel.PanelContainer.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
			this.radCollapsiblePanel.TabIndex = 0;
			this.radCollapsiblePanel.CollapsiblePanelElement.EnableAnimation = false;

			((Telerik.WinControls.UI.RadCollapsiblePanelElement)(this.radCollapsiblePanel.GetChildAt(0))).ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
			((Telerik.WinControls.UI.RadCollapsiblePanelElement)(this.radCollapsiblePanel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0);
			((Telerik.WinControls.UI.RadCollapsiblePanelElement)(this.radCollapsiblePanel.GetChildAt(0))).Margin = new System.Windows.Forms.Padding(0);
			((Telerik.WinControls.UI.CollapsiblePanelHeaderElement)(this.radCollapsiblePanel.GetChildAt(0).GetChildAt(1))).HorizontalHeaderAlignment = Telerik.WinControls.UI.RadHorizontalAlignment.Center;
			((Telerik.WinControls.UI.CollapsiblePanelHeaderElement)(this.radCollapsiblePanel.GetChildAt(0).GetChildAt(1))).Orientation = System.Windows.Forms.Orientation.Vertical;
			((Telerik.WinControls.UI.CollapsiblePanelButtonElement)(this.radCollapsiblePanel.GetChildAt(0).GetChildAt(1).GetChildAt(0))).TextOrientation = System.Windows.Forms.Orientation.Vertical;
			((Telerik.WinControls.UI.CollapsiblePanelButtonElement)(this.radCollapsiblePanel.GetChildAt(0).GetChildAt(1).GetChildAt(0))).Shape = this.tabEdgeShape;
			((Telerik.WinControls.UI.CollapsiblePanelButtonElement)(this.radCollapsiblePanel.GetChildAt(0).GetChildAt(1).GetChildAt(0))).StretchVertically = true;
			// 
			// radTreeView
			// 
			this.radTreeView.BackColor = System.Drawing.SystemColors.Control;
			this.radTreeView.Cursor = System.Windows.Forms.Cursors.Default;
			this.radTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radTreeView.ForeColor = System.Drawing.Color.Black;
			this.radTreeView.ItemHeight = 33;
			this.radTreeView.LineColor = System.Drawing.SystemColors.Window;
			this.radTreeView.LineWidth = 1.219184F;
			this.radTreeView.Location = new System.Drawing.Point(0, 0);
			this.radTreeView.Name = "radTreeView";
			this.radTreeView.ShowRootLines = false;
			this.radTreeView.Size = new System.Drawing.Size(377, 616);
			this.radTreeView.TabIndex = 0;
			this.radTreeView.TabStop = false;
			// 
			// radBreadCrumb
			// 
			this.radBreadCrumb.BackColor = System.Drawing.SystemColors.Control;
			this.radBreadCrumb.ChildMember = "";
			this.radBreadCrumb.DisplayMember = "";
			this.radBreadCrumb.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radBreadCrumb.IsTextModeEnabled = false;
			this.radBreadCrumb.Location = new System.Drawing.Point(413, 3);
			this.radBreadCrumb.Name = "radBreadCrumb";
			this.radBreadCrumb.ParentMember = "";
			this.radBreadCrumb.Size = new System.Drawing.Size(860, 38);
			this.radBreadCrumb.TabIndex = 2;
			this.radBreadCrumb.ValueMember = "";
			this.radBreadCrumb.DefaultTreeView = this.radTreeView;
			// 
			// statusControl
			// 
			this.statusControl.BackColor = System.Drawing.SystemColors.Control;
			this.statusControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusControl.Location = new System.Drawing.Point(0, 810);
			this.statusControl.Margin = new System.Windows.Forms.Padding(0);
			this.statusControl.Name = "statusControl";
			this.statusControl.Size = new System.Drawing.Size(1276, 40);
			this.statusControl.TabIndex = 5;
			// 
			// radStatusStrip
			// 
			this.radStatusStrip.Items.AddRange(new Telerik.WinControls.RadItem[] {
			StatusLabelElement});
			this.radStatusStrip.Location = new System.Drawing.Point(3, 853);
			this.radStatusStrip.Name = "radStatusStrip";
			this.radStatusStrip.Size = new System.Drawing.Size(1270, 25);
			this.radStatusStrip.TabIndex = 6;
			// 
			// radLabelElement
			// 
			StatusLabelElement.Name = "radLabelElement";
			this.radStatusStrip.SetSpring(StatusLabelElement, false);
			StatusLabelElement.Text = "Connection";
			StatusLabelElement.TextWrap = true;
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1276, 880);
			this.Controls.Add(this.tableLayoutPanelTop);
			this.Name = "MainForm";
			this.Text = "Каталог";
			this.MinimumSize = new System.Drawing.Size(800, 800);

			this.tableLayoutPanelTop.ResumeLayout(false);
			this.tableLayoutPanelTop.PerformLayout();
			this.tableLayoutPanelBottom.ResumeLayout(false);
			this.tableLayoutPanelBottom.PerformLayout();
			this.radCollapsiblePanel.PanelContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radTreeView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radBreadCrumb)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radStatusStrip)).EndInit();
			this.ResumeLayout(false);
		}

		#endregion
	}
}
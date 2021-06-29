namespace Catalog.Client
{
	partial class DetailsForm
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
			this.radTabbedFormControl = new Telerik.WinControls.UI.RadTabbedFormControl();
			this.ordersFormControlTab = new Telerik.WinControls.UI.RadTabbedFormControlTab();
			this.historyFormControlTab = new Telerik.WinControls.UI.RadTabbedFormControlTab();
			((System.ComponentModel.ISupportInitialize)(this.radTabbedFormControl)).BeginInit();
			this.radTabbedFormControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// radTabbedFormControl
			// 
			this.radTabbedFormControl.CaptionHeight = 20;
			this.radTabbedFormControl.Controls.Add(this.ordersFormControlTab);
			this.radTabbedFormControl.Controls.Add(this.historyFormControlTab);
			this.radTabbedFormControl.Location = new System.Drawing.Point(0, 0);
			this.radTabbedFormControl.Name = "radTabbedFormControl";
			// 
			// 
			// 
			this.radTabbedFormControl.RootElement.CustomFont = "Roboto";
			this.radTabbedFormControl.RootElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			this.radTabbedFormControl.SelectedTab = this.ordersFormControlTab;
			this.radTabbedFormControl.Size = new System.Drawing.Size(792, 595);
			this.radTabbedFormControl.TabHeight = 36;
			this.radTabbedFormControl.TabIndex = 0;
			this.radTabbedFormControl.TabSpacing = -1;
			this.radTabbedFormControl.Text = "DetailsForm2";
			// 
			// basketFormControlTab
			// 
			this.ordersFormControlTab.Location = new System.Drawing.Point(1, 37);
			this.ordersFormControlTab.Name = "basketFormControlTab";
			this.ordersFormControlTab.Size = new System.Drawing.Size(790, 557);
			// 
			// transactionHistoryFormControlTab
			// 
			this.historyFormControlTab.Location = new System.Drawing.Point(1, 37);
			this.historyFormControlTab.Name = "transactionHistoryFormControlTab";
			this.historyFormControlTab.Size = new System.Drawing.Size(790, 557);
			// 
			// DetailsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(792, 595);
			this.Controls.Add(this.radTabbedFormControl);
			this.Name = "DetailsForm";
			// 
			// 
			// 
			this.RootElement.ApplyShapeToControl = true;
			this.ShowIcon = false;
			this.Text = "";
			((System.ComponentModel.ISupportInitialize)(this.radTabbedFormControl)).EndInit();
			this.radTabbedFormControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.UI.RadTabbedFormControl radTabbedFormControl;
		private Telerik.WinControls.UI.RadTabbedFormControlTab ordersFormControlTab;
		private Telerik.WinControls.UI.RadTabbedFormControlTab historyFormControlTab;
	}
}

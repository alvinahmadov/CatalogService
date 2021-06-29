using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    partial class TableHeader
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.viewLabel = new Telerik.WinControls.UI.RadLabel();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewLabel)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Window;
			this.tableLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Controls.Add(this.viewLabel, 1, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(1044, 51);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// viewLabel
			// 
			this.viewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.viewLabel.Location = new System.Drawing.Point(420, 15);
			this.viewLabel.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.viewLabel.Name = "viewLabel";
			this.viewLabel.Size = new System.Drawing.Size(624, 21);
			this.viewLabel.TabIndex = 0;
			this.viewLabel.Text = "ViewLabel";
			// 
			// TableHeader
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "TableHeader";
			this.Size = new System.Drawing.Size(1044, 51);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewLabel)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private TableLayoutPanel tableLayoutPanel;
        private RadLabel viewLabel;
	}
}

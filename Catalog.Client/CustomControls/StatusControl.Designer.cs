using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    partial class StatusControl
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
            this.companyInfoLabel = new Telerik.WinControls.UI.RadLabel();
            this.updateStatusLabel = new Telerik.WinControls.UI.RadLabel();
            this.companyMailLabel = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.companyInfoLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateStatusLabel)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // companyInfoLabel
            // 

            var defaultFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.companyInfoLabel.AutoSize = false;
            this.companyInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.companyInfoLabel.Font = defaultFont;
            this.companyInfoLabel.Location = new System.Drawing.Point(0, 3);
            this.companyInfoLabel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.companyInfoLabel.Name = "companyInfoLabel";
            this.companyInfoLabel.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.companyInfoLabel.Size = new System.Drawing.Size(570, 33);
            this.companyInfoLabel.TabIndex = 0;
            // 
            // updateStatusLabel
            // 
            this.updateStatusLabel.AutoSize = false;
            this.updateStatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateStatusLabel.Location = new System.Drawing.Point(576, 3);
            this.updateStatusLabel.Name = "updateStatusLabel";
            this.updateStatusLabel.Size = new System.Drawing.Size(265, 33);
            this.updateStatusLabel.TabIndex = 1;
            this.updateStatusLabel.Font = defaultFont;
            this.updateStatusLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // companyMailLabel
            // 
            this.companyMailLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.companyMailLabel.Font = defaultFont;
            this.companyMailLabel.Location = new System.Drawing.Point(854, 3);
            this.companyMailLabel.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.companyMailLabel.Name = "companyMailLabel";
            this.companyMailLabel.Size = new System.Drawing.Size(182, 33);
            this.companyMailLabel.TabIndex = 2;
            this.companyMailLabel.TabStop = true;
            this.companyMailLabel.Tag = "www.google.com";
            this.companyMailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.companyMailLabel.LinkClicked += this.companyMailLabel_LinkClicked;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.92453F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.07547F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 201F));
            this.tableLayoutPanel.Controls.Add(this.companyInfoLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.companyMailLabel, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.updateStatusLabel, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1046, 39);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // StatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StatusControl";
            this.Size = new System.Drawing.Size(1046, 39);
            ((System.ComponentModel.ISupportInitialize)(this.companyInfoLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateStatusLabel)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RadLabel companyInfoLabel;
        private RadLabel updateStatusLabel;
        private LinkLabel companyMailLabel;
        private TableLayoutPanel tableLayoutPanel;
    }
}

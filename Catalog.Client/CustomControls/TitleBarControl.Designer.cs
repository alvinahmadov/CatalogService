using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    partial class TitleBarControl
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
            this.radTitleBar = new Telerik.WinControls.UI.RadTitleBar();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar)).BeginInit();
            this.SuspendLayout();
            // 
            // radTitleBar
            // 
            this.radTitleBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTitleBar.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar.Margin = new System.Windows.Forms.Padding(0, 0, 0, 50);
            this.radTitleBar.Name = "radTitleBar";
            this.radTitleBar.Size = new System.Drawing.Size(150, 50);
            this.radTitleBar.TabIndex = 0;
            this.radTitleBar.TabStop = false;
            // 
            // TitleBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radTitleBar);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TitleBarControl";
            this.Size = new System.Drawing.Size(150, 51);
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private RadPanel topPanel;
        private RadTitleBar radTitleBar;
    }
}

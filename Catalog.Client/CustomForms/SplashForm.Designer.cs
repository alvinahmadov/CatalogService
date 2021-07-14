namespace Catalog.Client
{
    partial class SplashForm
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
			this.components = new System.ComponentModel.Container();
			this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
			this.logoBox = new Telerik.WinControls.UI.RadPictureBox();
			this.statusLabel = new Telerik.WinControls.UI.RadLabel();
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
			this.logoBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusLabel)).BeginInit();
			this.SuspendLayout();
			// 
			// radPictureBox1
			// 
			this.logoBox.AllowPanelAnimations = false;
			this.logoBox.CausesValidation = false;
			this.logoBox.Controls.Add(this.statusLabel);
			this.logoBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logoBox.EnableAnalytics = false;
			this.logoBox.EnableGestures = false;
			this.logoBox.Image = global::Catalog.Client.Properties.Resources.startup;
			this.logoBox.ImageLayout = Telerik.WinControls.UI.RadImageLayout.FitIntoBounds;
			this.logoBox.Location = new System.Drawing.Point(0, 0);
			this.logoBox.Name = "radPictureBox1";
			this.logoBox.PanelOverflowMode = Telerik.WinControls.UI.PictureBoxPanelOverflowMode.VerticalOverHorizontal;
			this.logoBox.ShowBackground = true;
			this.logoBox.ShowItemToolTips = false;
			this.logoBox.ShowScrollBars = false;
			this.logoBox.Size = new System.Drawing.Size(682, 435);
			this.logoBox.SvgImageXml = null;
			this.logoBox.TabIndex = 0;
			this.logoBox.TabStop = false;
			this.logoBox.UseWaitCursor = true;
			// 
			// radLabel1
			// 
			this.statusLabel.AutoSize = false;
			this.statusLabel.BackColor = System.Drawing.Color.Transparent;
			this.statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.statusLabel.Location = new System.Drawing.Point(0, 389);
			this.statusLabel.Name = "radLabel1";
			this.statusLabel.Size = new System.Drawing.Size(682, 46);
			this.statusLabel.TabIndex = 0;
			this.statusLabel.TextWrap = false;
			this.statusLabel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.statusLabel.UseWaitCursor = true;
			// 
			// SplashForm
			// 
			this.AllowResize = false;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BorderAlignment = System.Drawing.Drawing2D.PenAlignment.Inset;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(682, 435);
			this.Controls.Add(this.logoBox);
			this.DoubleBuffered = true;
			this.EnableCompositionOnVista = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashForm";
			this.Shape = this.roundRectShape1;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.UseWaitCursor = true;
			((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
			this.logoBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusLabel)).EndInit();
			this.ResumeLayout(false);

        }

		#endregion

		private Telerik.WinControls.RoundRectShape roundRectShape1;
		private Telerik.WinControls.UI.RadPictureBox logoBox;
		private Telerik.WinControls.UI.RadLabel statusLabel;
	}
}

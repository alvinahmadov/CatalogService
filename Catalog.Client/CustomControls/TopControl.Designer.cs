using System.ComponentModel;
using System.Windows.Forms;

using Catalog.Client.Properties;

using Telerik.WinControls.UI;

namespace Catalog.Client
{
	partial class TopControl
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
			//
			// Init
			//
			this.radLogoBox = new RadPictureBox();
			this.radOrdersButton = new RadButton();
			this.radSettingsButton = new RadButton();
			this.radSearchTextBox = new SearchTextBox();
			this.radLayoutControl = new RadLayoutControl();

			this.radLayoutControl = new RadLayoutControl();
			this.radLogoBoxControlItem = new LayoutControlItem();
			this.radSettingsButtonControlItem = new LayoutControlItem();
			this.radSearchTextBoxControlItem = new LayoutControlItem();
			this.radOrdersButtonControlItem = new LayoutControlItem();

			((System.ComponentModel.ISupportInitialize)(this.radLayoutControl)).BeginInit();
			this.radLayoutControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.radSearchTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLogoBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radOrdersButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radSettingsButton)).BeginInit();
			this.SuspendLayout();
			// 
			// radLayoutControl
			// 
			this.radLayoutControl.Controls.Add(this.radSearchTextBox);
			this.radLayoutControl.Controls.Add(this.radLogoBox);
			this.radLayoutControl.Controls.Add(this.radOrdersButton);
			this.radLayoutControl.Controls.Add(this.radSettingsButton);
			this.radLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radLayoutControl.Location = new System.Drawing.Point(0, 0);
			this.radLayoutControl.Margin = new System.Windows.Forms.Padding(0);
			this.radLayoutControl.MinimumSize = new System.Drawing.Size(500, 100);
			this.radLayoutControl.Name = "radLayoutControl";
			this.radLayoutControl.RootElement.MinSize = new System.Drawing.Size(500, 100);
			this.radLayoutControl.Size = new System.Drawing.Size(1324, 100);
			this.radLayoutControl.TabIndex = 0;
			this.radLayoutControl.Items.AddRange(new Telerik.WinControls.RadItem[] {
				this.radLogoBoxControlItem,
				this.radSearchTextBoxControlItem,
				this.radSettingsButtonControlItem,
				this.radOrdersButtonControlItem,
				}
			);
			// 
			// radSearchTextBox
			// 
			this.radSearchTextBox.Font = new System.Drawing.Font("Segoe UI", 12.26415F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radSearchTextBox.Location = new System.Drawing.Point(260, 19);
			this.radSearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.radSearchTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.radSearchTextBox.Location = new System.Drawing.Point(300, 40);
			this.radSearchTextBox.Name = "searchTextBox";
			this.radSearchTextBox.Padding = new System.Windows.Forms.Padding(0);
			this.radSearchTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.radSearchTextBox.ShowClearButton = true;
			this.radSearchTextBox.ShowNullText = true;
			this.radSearchTextBox.Size = new System.Drawing.Size(674, 19);
			this.radSearchTextBox.TabIndex = 5;
			this.radSearchTextBox.RootElement.MaxSize = new System.Drawing.Size(0, 60);
			this.radSearchTextBox.RootElement.MinSize = new System.Drawing.Size(0, 60);
			this.radSearchTextBox.Size = new System.Drawing.Size(195, 60);
			// 
			// radSearchTextBoxControlItem
			// 
			this.radSearchTextBoxControlItem.AssociatedControl = this.radSearchTextBox;
			this.radSearchTextBoxControlItem.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.radSearchTextBoxControlItem.AllowHide = false;
			this.radSearchTextBoxControlItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
			this.radSearchTextBoxControlItem.Bounds = new System.Drawing.Rectangle(300, 0, 674, 100);
			this.radSearchTextBoxControlItem.ControlVerticalAlignment = RadVerticalAlignment.Center;
			this.radSearchTextBoxControlItem.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
			this.radSearchTextBoxControlItem.Margin = new System.Windows.Forms.Padding(0);
			this.radSearchTextBoxControlItem.MaxSize = new System.Drawing.Size(0, 100);
			this.radSearchTextBoxControlItem.MinSize = new System.Drawing.Size(300, 26);
			this.radSearchTextBoxControlItem.Name = "radSearchTextBoxControlItem";
			this.radSearchTextBoxControlItem.Padding = new System.Windows.Forms.Padding(50, 0, 100, 0);
			this.radSearchTextBoxControlItem.StretchHorizontally = false;
			this.radSearchTextBoxControlItem.StretchVertically = true;
			this.radSearchTextBoxControlItem.Text = "";
			// 
			// radLogoPictureBox
			// 
			this.radLogoBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.radLogoBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radLogoBox.Image = global::Catalog.Client.Properties.Resources.market_logo1;
			this.radLogoBox.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.radLogoBox.Location = new System.Drawing.Point(3, 3);
			this.radLogoBox.Name = "radLogoPictureBox";
			this.radLogoBox.PanelDisplayMode = PictureBoxPanelDisplayMode.Always;
			this.radLogoBox.ShowBackground = true;
			this.radLogoBox.ShowScrollBars = false;
			this.radLogoBox.Size = new System.Drawing.Size(294, 98);
			this.radLogoBox.SvgImageXml = null;
			this.radLogoBox.TabIndex = 3;

			// 
			// radOrdersButton
			// 
			this.radOrdersButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radOrdersButton.EnableAnalytics = false;
			this.radOrdersButton.EnableGestures = false;
			this.radOrdersButton.Location = new System.Drawing.Point(977, 3);
			this.radOrdersButton.Name = "radOrdersButton";
			this.radOrdersButton.Size = new System.Drawing.Size(194, 94);
			this.radOrdersButton.TabIndex = 8;
			this.radOrdersButton.Text = "";
			this.radOrdersButton.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
			this.radOrdersButton.TextWrap = true;
			this.radOrdersButton.Image = Resources.basket;
			this.radOrdersButton.TextImageRelation = TextImageRelation.TextBeforeImage;
			this.radOrdersButton.ImageAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.radOrdersButton.BackColor = System.Drawing.Color.OldLace;
			// 
			// radOrdersButtonControlItem
			// 
			this.radOrdersButtonControlItem.AssociatedControl = this.radOrdersButton;
			this.radOrdersButtonControlItem.Bounds = new System.Drawing.Rectangle(974, 0, 200, 100);
			this.radOrdersButtonControlItem.MaxSize = new System.Drawing.Size(250, 100);
			this.radOrdersButtonControlItem.MinSize = new System.Drawing.Size(150, 100);
			this.radOrdersButtonControlItem.Name = "radOrdersButtonControlItem";
			this.radOrdersButtonControlItem.Text = "layoutControlItem1";
			// 
			// radLogoPictureBoxControlItem
			// 
			this.radLogoBoxControlItem.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.radLogoBoxControlItem.AllowHide = false;
			this.radLogoBoxControlItem.AssociatedControl = this.radLogoBox;
			this.radLogoBoxControlItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
			this.radLogoBoxControlItem.BorderBottomWidth = 0F;
			this.radLogoBoxControlItem.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
			this.radLogoBoxControlItem.BorderLeftWidth = 0F;
			this.radLogoBoxControlItem.BorderRightWidth = 0F;
			this.radLogoBoxControlItem.BorderTopWidth = 0F;
			this.radLogoBoxControlItem.BorderWidth = 0F;
			this.radLogoBoxControlItem.Bounds = new System.Drawing.Rectangle(0, 0, 400, 100);
			this.radLogoBoxControlItem.CanFocus = false;
			this.radLogoBoxControlItem.ControlVerticalAlignment = RadVerticalAlignment.Top;
			this.radLogoBoxControlItem.DrawImage = true;
			this.radLogoBoxControlItem.EnableFocusBorderAnimation = false;
			this.radLogoBoxControlItem.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentBounds;
			this.radLogoBoxControlItem.FocusBorderColor = System.Drawing.Color.Transparent;
			this.radLogoBoxControlItem.MaxSize = new System.Drawing.Size(400, 100);
			this.radLogoBoxControlItem.MinSize = new System.Drawing.Size(200, 100);
			this.radLogoBoxControlItem.Name = "radLogoPictureBoxControlItem";
			this.radLogoBoxControlItem.ShadowColor = System.Drawing.Color.Transparent;
			this.radLogoBoxControlItem.Text = "";
			// 
			// radSettingsButton
			// 
			this.radSettingsButton.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.radSettingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.radSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.radSettingsButton.EnableAnalytics = false;
			this.radSettingsButton.EnableGestures = false;
			this.radSettingsButton.Image = global::Catalog.Client.Properties.Resources.settings;
			this.radSettingsButton.Location = new System.Drawing.Point(1177, 3);
			this.radSettingsButton.Name = "radSettingsButton";
			this.radSettingsButton.Size = new System.Drawing.Size(144, 94);
			this.radSettingsButton.TabIndex = 7;
			this.radSettingsButton.Text = "Настройки";
			this.radSettingsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.radSettingsButton.ThemeName = "Fluent";
			// 
			// radSettingsButtonControlItem
			// 
			this.radSettingsButtonControlItem.AssociatedControl = this.radSettingsButton;
			this.radSettingsButtonControlItem.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
			this.radSettingsButtonControlItem.Bounds = new System.Drawing.Rectangle(1174, 0, 150, 100);
			this.radSettingsButtonControlItem.MaxSize = new System.Drawing.Size(200, 100);
			this.radSettingsButtonControlItem.MinSize = new System.Drawing.Size(120, 100);
			this.radSettingsButtonControlItem.Name = "radSettingsButtonControlItem";
			this.radSettingsButtonControlItem.Text = "";
			// 
			// TopControl2
			// 
			this.Controls.Add(this.radLayoutControl);
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.MaximumSize = new System.Drawing.Size(0, 100);
			this.MinimumSize = new System.Drawing.Size(900, 100);
			this.Size = new System.Drawing.Size(1324, 100);
			this.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.Name = "TopControl";

			((System.ComponentModel.ISupportInitialize)(this.radLayoutControl)).EndInit();
			this.radLayoutControl.ResumeLayout(false);
			this.radLayoutControl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.radSearchTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLogoBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radOrdersButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radSettingsButton)).EndInit();
			this.ResumeLayout(false);
		}

		#endregion
	}
}

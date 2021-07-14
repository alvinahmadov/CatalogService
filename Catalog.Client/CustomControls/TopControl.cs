using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

using Telerik.WinControls.UI;

using DBSettings = Catalog.Common.Service.Settings;

namespace Catalog.Client
{
	public partial class TopControl : UserControl
	{
		public RadButton OrdersButton
		{
			get => this.radOrdersButton;
		}

		public SearchTextBox SearchTextBox
		{
			get => this.radSearchTextBox;
		}

		public TopControl()
		{
			this.Visible = false;
			InitializeComponent();
			this.radLayoutControl.Visible = false;

			this.radSettingsDialog = new SettingsTabbedForm();
			this.radSettingsButton.Click += RadSettingsButton_Click;

			this.radSearchTextBox.Size = new Size(200, 100);
			this.radSearchTextBox.Location = new Point(10, 200);
			this.radSearchTextBox.NullText = "Найти";

			this.Margin = new Padding(0);
			this.VisibleChanged += TopControl_VisibleChanged;
		}

		private void TopControl_VisibleChanged(Object sender, EventArgs e)
		{
			this.radLayoutControl.Visible = this.Visible;
		}

		private void TopControl_Load(Object sender, EventArgs e)
		{
			this.radSettingsDialog.Hide();
		}

		private void RadSettingsButton_Click(Object sender, EventArgs e)
		{
			this.radSettingsDialog.Show();
		}

		#region Private members

		[SuppressMessage("Style", "IDE0044")]
		private RadPictureBox radLogoBox;
		
		[SuppressMessage("Style", "IDE0044")]
		private RadButton radOrdersButton;
		
		[SuppressMessage("Style", "IDE0044")]
		private RadButton radSettingsButton;
		
		[SuppressMessage("Style", "IDE0044")]
		private SearchTextBox radSearchTextBox;
		
		[SuppressMessage("Style", "IDE0044")]
		private RadLayoutControl radLayoutControl;

		[SuppressMessage("Style", "IDE0044")]
		private LayoutControlItem radLogoBoxControlItem;
		
		[SuppressMessage("Style", "IDE0044")]
		private LayoutControlItem radSettingsButtonControlItem;

		[SuppressMessage("Style", "IDE0044")]
		private LayoutControlItem radSearchTextBoxControlItem;
		
		[SuppressMessage("Style", "IDE0044")]
		private LayoutControlItem radOrdersButtonControlItem;

		[SuppressMessage("Style", "IDE0044")]
		private SettingsTabbedForm radSettingsDialog;

		#endregion
	}
}

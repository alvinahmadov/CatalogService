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

		public RadLabel UpdateStatus { get; set; }

		public TopControl(Control parent)
		{
			this.Parent = parent;
			this.Visible = false;
			InitializeComponent();

			this.radLayoutControl.Visible = false;

			this.radSettingsDialog = new SettingsTabbedForm();
			this.radSettingsDialog.UpdateStatusLabel = UpdateStatus;
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

		private RadButton radOrdersButton;
		private RadButton radSettingsButton;
		private RadPictureBox radLogoBox;
		private SearchTextBox radSearchTextBox;
		private RadLayoutControl radLayoutControl;
		private LayoutControlItem radLogoBoxControlItem;
		private LayoutControlItem radSettingsButtonControlItem;
		private LayoutControlItem radSearchTextBoxControlItem;
		private LayoutControlItem radOrdersButtonControlItem;
		private readonly SettingsTabbedForm radSettingsDialog;

		#endregion
	}
}

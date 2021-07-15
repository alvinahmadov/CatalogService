using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Telerik.WinControls.UI;
using Telerik.WinControls;

using Catalog.Client.Properties;
using Catalog.Common.Repository;
using Catalog.Common;

namespace Catalog.Client
{
	public partial class SettingsTabbedForm : RadTabbedForm
	{
		public RadLabel UpdateStatusLabel { get; set; }

		public SettingsTabbedForm()
		{
			InitializeComponent();
			this.AllowAero = false;
			this.ShowInTaskbar = true;
			this.AcceptButton = this.submitButton;
			this.CancelButton = this.cancelButton;
			this.VisibleChanged += SettingsTabbedForm_VisibleChanged;

			this.radConfirmCheckBox.Checked = Settings.Default.AskConfirmation;
			this.radLoadImageCheckBox.Checked = Settings.Default.LoadImage;
			this.updateItervalSpinControl.Value = Settings.Default.UpdateInterval;
			this.radLeftPanelWithSpin.Value = Settings.Default.LeftPanelWidth;
			this.updateButton.Click += UpdateButton_Click;
			this.submitButton.Click += SubmitButton_Click;
			this.cancelButton.Click += CancelButton_Click;
			this.loginButton.Click += LoginButton_Click;
			this.registrationLink.LinkClicked += RegistrationLinkClickedEvent;
			this.forgotLinkLabel.LinkClicked += ForgotLinkClickedEvent;

			this.radTabFormControl.SelectedTab = this.radCommonTab;
		}

		private void SettingsTabbedForm_VisibleChanged(Object sender, EventArgs e)
		{
			this.updateButton.Enabled = WebWorker.UpdateStatus != Common.UpdateStatus.Started;
		}

		private void UpdateStatus(string status, Object statusTextAlignment = null)
		{
			Invoke(new Action(() => MainForm.StatusLabelElement.Text = status));
		}

		private void UpdateButton_Click(Object sender, EventArgs e)
		{
			new Thread(new ThreadStart(UpdateData)).Start();
		}

		private void SubmitButton_Click(Object sender, EventArgs e)
		{
			var updateInterval = Convert.ToInt32(this.updateItervalSpinControl.Value);
			var maxWidth = Convert.ToInt16(this.radLeftPanelWithSpin.Value);
			Settings.Default.AskConfirmation = this.radConfirmCheckBox.Checked;
			Settings.Default.LoadImage = this.radLoadImageCheckBox.Checked;

			if (maxWidth > GUI.COLLAPSIBLE_PANEL_MIN_WIDTH && maxWidth < GUI.COLLAPSIBLE_PANEL_MAX_WIDTH)
			{
				Settings.Default.LeftPanelWidth = maxWidth;
				MainForm.CollapsiblePanelMaxWidth = maxWidth;

				((this.Parent as TopControl)?.Parent as MainForm)?.UpdateCollabsiblePanel();
			}

			if (updateInterval >= 5 || updateInterval == 0)
			{
				Settings.Default.UpdateInterval = updateInterval;
				if (updateInterval == 0)
				{
					UpdateTimer.Stop();
				}
				else
				{
					if (!UpdateTimer.IsEnabled)
						UpdateTimer.Start();
					UpdateTimer.Change(Settings.Default.UpdateInterval);
				}
			}
			else
				RadMessageBox.Show("Допускается время не менее 5 минут. При значении 0 автообновление отключается");

			Settings.Default.Commit();

			Hide();
		}

		private void CancelButton_Click(Object sender, EventArgs e)
		{
			Hide();
		}

		private void LoginButton_Click(Object sender, EventArgs e)
		{

		}

		private void UpdateData()
		{
			if (WebWorker.HasConnection
				&& WebWorker.UpdateStatus != Common.UpdateStatus.Started)
			{
				Invoke(new Action(() => this.updateButton.Enabled = false));
				WebWorker.LoggingCallback = UpdateStatus;
				WebWorker.SuccessCallback = new Action(() =>
				{
					this.updateButton.Enabled = true;
					UpdateStatusLabel.Text = string.Format(MESSAGE.Gui.UPDATE_STATUS, DateTime.Now);
				});
				WebWorker.LoadData();
			}
			else
				RadMessageBox.Show(MESSAGE.Web.NO_CONNECTION);
		}

		private void RegistrationLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Settings.Default.authURL);
		}

		private void ForgotLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Settings.Default.authURL);
		}
	}
}

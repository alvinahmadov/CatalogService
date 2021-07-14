using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

using Telerik.WinControls.UI;
using Telerik.WinControls;

using Catalog.Client.Properties;
using Catalog.Common.Repository;

namespace Catalog.Client
{
	public partial class SettingsTabbedForm : RadTabbedForm
	{
		public SettingsTabbedForm()
		{
			InitializeComponent();

			this.AllowAero = false;
			this.ShowInTaskbar = true;
			this.AcceptButton = this.submitButton;
			this.CancelButton = this.cancelButton;

			this.radConfirmCheckBox.Checked = Settings.Default.AskConfirmation;
			this.radLoadImageCheckBox.Checked = Settings.Default.LoadImage;
			this.updateItervalSpinControl.Value = Settings.Default.UpdateInterval;

			this.updateButton.Click += UpdateButton_Click;
			this.submitButton.Click += SubmitButton_Click;
			this.cancelButton.Click += CancelButton_Click;
			this.loginButton.Click += LoginButton_Click;
			this.registrationLink.LinkClicked += RegistrationLinkClickedEvent;
			this.forgotLinkLabel.LinkClicked += ForgotLinkClickedEvent;

			this.radTabFormControl.SelectedTab = this.radCommonTab;

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
			Settings.Default.AskConfirmation = this.radConfirmCheckBox.Checked;
			Settings.Default.LoadImage = this.radLoadImageCheckBox.Checked;
			Settings.Default.UpdateInterval = Convert.ToInt32(this.updateItervalSpinControl.Value);
			Settings.Default.Commit();
			Close();
		}

		private void CancelButton_Click(Object sender, EventArgs e)
		{
			Close();
		}

		private void LoginButton_Click(Object sender, EventArgs e)
		{

		}

		private void UpdateData()
		{
			if (WebWorker.HasConnection
				&& WebWorker.UpdateStatus != Common.UpdateStatus.Started)
			{
				Invoke(new Action(() => { this.updateButton.Enabled = false; }));
				WebWorker.LoggingCallback = UpdateStatus;
				WebWorker.UpdateStatus = Common.UpdateStatus.Started;
				WebWorker.InitProductCategories();
				WebWorker.InitProductSubcategories();
				UpdateStatus("Обновление товаров...");
				var count = WebWorker.InitProducts();
				if (Settings.Default.LoadImage && !WebWorker.PhotosUpdating)
				{
					UpdateStatus("Обновление картинок...");
					Task.Run(WebWorker.InitProductPhotos);
				}

				MainRepository.ResetCache();
				WebWorker.UpdateStatus = Common.UpdateStatus.Finished;

				UpdateStatus($"<html>Обновлено <b>{count}</b> товаров");

				WebWorker.LoggingCallback = null;

				Invoke(new Action(() => { this.updateButton.Enabled = true; }));
			}
			else
				RadMessageBox.Show("Отсутсвует соединение. Повторите позже.");
		}

		private void RegistrationLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Settings.Default.authURL);
		}

		private void ForgotLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Properties.Settings.Default.authURL);
		}
	}
}

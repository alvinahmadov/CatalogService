using System;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls.UI;

using Catalog.Client.Properties;
using Catalog.Common.Repository;
using System.Threading.Tasks;
using Telerik.WinControls;

namespace Catalog.Client
{
	public partial class SettingsTabbedForm : RadTabbedForm
	{
		public SettingsTabbedForm()
		{
			InitializeComponent();

			this.AllowAero = false;
			this.ShowInTaskbar = false;
			this.AcceptButton = this.submitButton;
			this.CancelButton = this.cancelButton;

			this.radConfirmCheckBox.Checked = Settings.AskConfirmation;
			this.radLoadImageCheckBox.Checked = Settings.LoadImages;
			this.updateItervalSpinControl.Value = Settings.UpdateInterval;

			this.updateButton.Click += UpdateButton_Click;
			this.submitButton.Click += SubmitButton_Click;
			this.cancelButton.Click += CancelButton_Click;
			this.loginButton.Click += LoginButton_Click;
			this.registrationLink.LinkClicked += RegistrationLinkClickedEvent;
			this.forgotLinkLabel.LinkClicked += ForgotLinkClickedEvent;

			this.radTabFormControl.SelectedTab = this.radCommonTab;


			this.updateItervalSpinControl.Value = Properties.Settings.UpdateInterval;
		}
		private void UpdateStatus(string status)
		{
			Invoke(new Action(() => MainForm.StatusLabelElement.Text = status));
		}

		private async void UpdateButton_Click(Object sender, EventArgs e)
		{
			if (WebRepository.HasConnection)
			{
				await Task.Run(() =>
				{
					Invoke(new Action(() => MainForm.StatusLabelElement.Text = "Обновление..."));
					WebRepository.UpdateRequested = true;
					UpdateStatus("Обновление категорий...");
					WebRepository.GetProductCategories();
					WebRepository.GetProductSubcategories();
					UpdateStatus("Обновление товаров...");
					WebRepository.GetProducts();
					WebRepository.GetProductInventories();
					MainRepository.ResetCache(CacheType.INVENTORY);
					WebRepository.UpdateRequested = false;

					UpdateStatus($"<html>Обновлено <b>{WebRepository.ProductInventoryUpdateCount}</b> товаров");
				});
			}
			else
				RadMessageBox.Show("Отсутсвует соединение. Повторите позже.");
		}

		private void SubmitButton_Click(Object sender, EventArgs e)
		{
			Settings.AskConfirmation = this.radConfirmCheckBox.Checked;
			Settings.LoadImages = this.radLoadImageCheckBox.Checked;
			Settings.UpdateInterval = Convert.ToInt32(this.updateItervalSpinControl.Value);
			Settings.Commit();
			Hide();
		}

		private void CancelButton_Click(Object sender, EventArgs e)
		{
			Hide();
		}

		private void LoginButton_Click(Object sender, EventArgs e)
		{

		}

		private void RegistrationLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Settings.Default.authURL);
		}

		private void ForgotLinkClickedEvent(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(Settings.Default.authURL);
		}
	}
}

using System;
using System.Windows.Forms;

using Telerik.WinControls.UI;

namespace Catalog.Client
{
	public partial class DetailsForm : RadTabbedForm
	{
		static FormWindowState currentWindowState = FormWindowState.Maximized;

		private BaseGridControl shoppingControl;

		private BaseGridControl historyControl;

		public DetailsForm()
		{
			InitializeComponent();
			this.ordersFormControlTab.Text = "Список заказов";
			this.historyFormControlTab.Text = "История заказов";

			this.radTabbedFormControl.ShowTabCloseButton = false;
			this.radTabbedFormControl.ShowNewTabButton = false;
			WindowState = FormWindowState.Maximized;
			radTabbedFormControl.SelectedTabChanging += RadTabbedFormControl_SelectedTabChanging;
			AttachCartTab(true);
			AttactTransactionsTab();
		}


		private void DetailsForm_Resize(Object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == currentWindowState)
			{
				historyControl?.RefreshData();
				currentWindowState = FormWindowState.Maximized;
			}

			if (WindowState == FormWindowState.Minimized)
				currentWindowState = FormWindowState.Minimized;
		}

		private void RadTabbedFormControl_SelectedTabChanging(Object sender, RadTabbedFormControlCancelEventArgs e)
		{
			if (e.Tab == historyFormControlTab)
			{
				historyControl.RefreshData();
			}
		}

		public void AttachCartTab(bool selected = false)
		{
			this.shoppingControl = new ShoppingCartControl();
			this.ordersFormControlTab.Controls.Add(this.shoppingControl);

			if (selected)
				Select();
		}

		public void AttactTransactionsTab(bool selected = false)
		{
			this.historyControl = new TransactionHistoryControl();
			this.historyFormControlTab.Controls.Add(this.historyControl);

			if (selected)
				Select();
		}
	}
}

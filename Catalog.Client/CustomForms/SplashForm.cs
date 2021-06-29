using System;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Threading.Tasks;

using Catalog.Common.Repository;
using System.Diagnostics;

namespace Catalog.Client
{
	public partial class SplashForm : ShapedForm
	{
		public enum WorkType
		{
			FETCH = 0,
			WAIT = 1
		}

		public RadLabel StatusLabel { get; private set; }

		public WorkType OperationMode { get; private set; }

		public SplashForm(string message = null, WorkType mode = WorkType.FETCH)
		{
			InitializeComponent();
			StatusLabel = this.statusLabel;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.logoBox.ContextMenuEnabled = false;
			this.OperationMode = mode;
			this.TopLevel = true;
			if (message != null)
				this.StatusLabel.Text = message;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (this.OperationMode is WorkType.FETCH)
				new Thread(new ThreadStart(FetchData)).Start();
		}

		private void UpdateStatus(string status) 
		{
			Invoke(new Action(() => this.statusLabel.Text = status));
		}

		public void FetchData()
		{
			if (!WebRepository.HasConnection)
			{
				UpdateStatus("Отсутствует подключение к сети!");
				Thread.Sleep(1500);
			}
			try
			{
				WebRepository.UpdateRequested = true;
				UpdateStatus("Обновление категорий...");
				WebRepository.GetProductCategories();
				WebRepository.GetProductSubcategories();
				UpdateStatus("Обновление товаров...");
				WebRepository.GetProducts();
				WebRepository.GetProductInventories();

				if (Properties.Settings.LoadImages && WebRepository.HasConnection) 
				{
					UpdateStatus("Обновление картинок...");
					Task.Run(WebRepository.GetProductPhotos);
				}

				MainRepository.ResetCache(CacheType.INVENTORY);
				UpdateStatus($"<html>Обновлено <b>{MainRepository.ProductInventoriesCache.Count}</b> товаров");
				WebRepository.UpdateRequested = false;
			}
			catch (Exception ex)
			{
				UpdateStatus($"<html><span style=\"color:red;\">Произошла ошибка: {ex.Message}</span>");
				MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Connection Error");
			}
			
			try 
			{
				BaseGridControl.ProductInventories.Init();
			} catch (Exception e)
			{
				Debug.WriteLine($"Exception in SplashForm: {e.Message}\nStackTrace: {e.StackTrace}");
			}

			Invoke(new Action(Close));
			Dispose();
		}
	}
}

using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Telerik.WinControls.UI;

using Catalog.Common;
using Catalog.Common.Repository;
using Catalog.Client.Properties;

namespace Catalog.Client
{
	public partial class MainForm : RadForm
	{
		static readonly int CollapsiblePanelMaxWidth = 300;

		static readonly int CollapsiblePanelMinWidth = 35;

		static Common.UpdateTimer UpdateTimer;

		public MainForm()
		{
			InitializeComponent();
			this.Icon = Resources.logo14;
			this.Text = "СБМ-Волга Каталог";
			this.Name = "СБМ-Волга";
			this.AllowTheming = true;
			this.ShowIcon = false;
			this.StartPosition = FormStartPosition.CenterScreen;

			this.controlCollection = new GridControlCollection();
			StatusLabelElement.Text = "Инициализация...";
			this.radCollapsiblePanel.EnableAnimation = false;
			this.radTreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
			this.radCollapsiblePanel.Collapsed += CollapsiblePanel_Collapsed;
			this.radCollapsiblePanel.Expanded += CollapsiblePanel_Expanded;
			this.topControl.OrdersButton.Click += OrdersButton_Click;

			InitializeControls();
		}

		~MainForm()
		{
			try
			{
				Repository.Context.Database.Connection.Close();
				UpdateTimer.Stop();
			}
			catch
			{
				Debug.WriteLine("Exception on closing connection");
			}
		}

		protected void InitializeControls()
		{
			BaseGridControl.TopButton = this.topControl.OrdersButton;
			BaseGridControl.SearchTextBox = this.topControl.SearchTextBox;

			void updateCallback()
			{
				Debug.WriteLine("Update testing");
				WebWorker.LoggingCallback = UpdateStatus;
				if (!WebWorker.HasConnection)
				{
					UpdateStatus("Отсутствует подключение к сети!");
					Thread.Sleep(1500);
					return;
				}

				if (WebWorker.UpdateStatus == Common.UpdateStatus.Started)
					return;

				try
				{
					
					WebWorker.UpdateStatus = Common.UpdateStatus.Started;
					WebWorker.InitProductCategories();
					WebWorker.InitProductSubcategories();
					var productCount = WebWorker.InitProducts();

					if (Settings.Default.LoadImage && WebWorker.HasConnection
						&& !WebWorker.PhotosUpdating)
					{
						UpdateStatus("Обновление картинок...");
						Task.Run(WebWorker.InitProductPhotos);
					}

					MainRepository.ResetCache(CacheType.INVENTORY);
					UpdateStatus($"<html>Обновлено <b>{productCount}</b> товаров");
					WebWorker.UpdateStatus = Common.UpdateStatus.Finished;
				}
				catch (Exception ex)
				{
					UpdateStatus($"<html><span style=\"color:red;\">Произошла ошибка: {ex.Message}</span>");
					MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Connection Error");
				}
			}

			var interval = Settings.Default.UpdateInterval;

			UpdateTimer = new Common.UpdateTimer(interval, updateCallback);

			var builder = new NodeBuilder(radTreeView, FontStyle.Bold);
			var absTag = ProductTag.Absolute;
			placeholderCtrl = new InventoriesControl(absTag);
			this.tableLayoutPanelBottom.Controls.Add(placeholderCtrl, 1, 1);

			var treeNodes = builder.InitializeNodes(false, "Все", tag =>
			{
				var ctrl = new InventoriesControl(tag);
				controlCollection.AddControl(ctrl, tag);
			});

			try
			{
				if (InventoryCollection.StatusButton == null)
				{
					InventoryCollection.StatusButton = this.topControl.OrdersButton;
					try
					{
						var cart = MainRepository.GetShoppingCarts().SingleOrDefault();

						if (cart != null)
						{
							var price = CultureConfig.CurrencyInfo.Format(cart.TotalPrice);
							this.topControl.OrdersButton.Text =
								String.Format(
									MESSAGE.Gui.ORDERS_STATUS_TEMPLATE,
									cart.TotalQuantity,
									price
								);
						}
					}
					catch { }
				}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
			}

			radTreeView.Nodes.AddRange(treeNodes.Values);
			this.WindowState = FormWindowState.Maximized;
			this.topControl.Visible = true;
			UpdateTimer.Start();
		}

		void UpdateStatus(string statusText, Object obj = null)
		{
			Invoke(new Action(() => StatusLabelElement.Text = statusText));
		}

		#region Event handling

		private void CollapsiblePanel_Expanded(object sender, EventArgs e)
		{
			tableLayoutPanelBottom.ColumnStyles[0].Width = CollapsiblePanelMaxWidth;
		}

		private void CollapsiblePanel_Collapsed(object sender, EventArgs e)
		{
			tableLayoutPanelBottom.ColumnStyles[0].Width = CollapsiblePanelMinWidth;
		}

		protected virtual void OrdersButton_Click(Object sender, EventArgs e)
		{
			var ctrl = controlCollection.ActiveControl.Control as InventoriesControl;
			var dialog = new DetailsForm { Text = "Корзина" };

			dialog.FormClosed +=
			(s, fcea) =>
			{
				controlCollection
							 .Controls
							 .ForEach(c => c.Control.GridControl.TableElement.SynchronizeRows());
				placeholderCtrl.GridControl.TableElement.SynchronizeRows();
			};
			dialog.Show();
		}

		private void TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
		{
			ProductTag tag = e.Node.Tag as ProductTag;

			if (tag.Count == 0)
			{
				if (tag.CID == 0)
				{
					tag.Count = MainRepository.ProductsCache.Count;
				}
				else if (tag.SID == -1)
				{

					var subcaches = MainRepository.ProductSubcategoriesCache
												  .Where(sc => sc.ProductCategoryID == tag.CID);

					foreach (var subcat in subcaches)
						tag.Count += subcat.Products.Count;
				}
				else
				{
					tag.Count = MainRepository.ProductInventoriesCache
											  .Count(p => p.ProductSubcategoryID == tag.SID);
				}
			}
			AttachGridControl(tag);
		}

		#endregion

		#region Modules

		public void AttachGridControl(ProductTag tag)
		{
			try
			{
				InventoriesControl newCtrl = null;
				ControlContainer currentHldr = null;

				if (tag.IsZero || tag.SID == -1) // Common
				{
					currentHldr = controlCollection[tag.CID] as ParentControlContainer;
				}
				else // Only by subcategory
				{
					currentHldr = controlCollection.Get(tag.CID, tag.SID) as ChildControlContainer;
				}

				currentHldr.IsCurrentControl = true;
				controlCollection.ActiveControl = currentHldr;
				newCtrl = currentHldr.Control as InventoriesControl;
				var oldCtrl = tableLayoutPanelBottom.GetControlFromPosition(1, 1);

				if (oldCtrl != null)
				{
					var oldTag = oldCtrl.Tag as ProductTag;

					if (oldTag.CID == Int32.MinValue && oldTag.SID == Int32.MinValue)
						tableLayoutPanelBottom.Controls.Remove(oldCtrl);
					else
					{
						var oldHldr = controlCollection.Find(oldTag.CID, oldTag.SID);
						oldHldr.IsCurrentControl = false;
						tableLayoutPanelBottom.Controls.Remove(oldCtrl);
					}
				}
				tableLayoutPanelBottom.Controls.Add(newCtrl, 1, 1);
				radTreeView.SelectedNode.Value = tag.ToString();
				newCtrl.ResetFilter(true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"{ex.Message}: {ex?.StackTrace}");
			}
		}

		#endregion

		#region Fields
		readonly GridControlCollection controlCollection;
		private BaseGridControl placeholderCtrl;

		private TopControl topControl;
		private StatusControl statusControl;
		private TableLayoutPanel tableLayoutPanelTop;
		private TableLayoutPanel tableLayoutPanelBottom;
		private RadStatusStrip radStatusStrip;
		private RadCollapsiblePanel radCollapsiblePanel;
		private RadTreeView radTreeView;
		private RadBreadCrumb radBreadCrumb;
		private TabEdgeShape tabEdgeShape;
		public static RadLabelElement StatusLabelElement;

		#endregion
	}

}
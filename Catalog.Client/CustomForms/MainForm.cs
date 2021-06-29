using Catalog.Client.Properties;
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Catalog.Common.Utils;
using Catalog.Common.Repository;

namespace Catalog.Client
{
	public partial class MainForm : RadForm
	{
		static readonly int CollapsiblePanelMaxWidth = 300;

		static readonly int CollapsiblePanelMinWidth = 35;

		public MainForm()
		{
			InitializeComponent();
			this.Icon = Resources.ERP;
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

		protected void InitializeControls()
		{
			BaseGridControl.TopButton = this.topControl.OrdersButton;
			BaseGridControl.SearchTextBox = this.topControl.SearchTextBox;

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
									StatusTemplate.ORDERS_STATUS_TEMPLATE,
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
				BaseGridControl newCtrl = null;
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
				newCtrl = currentHldr.Control;
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
				(newCtrl as InventoriesControl).ResetFilter();
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
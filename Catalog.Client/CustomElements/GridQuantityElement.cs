using System;
using System.Drawing;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;

using Catalog.Client.Properties;

using InventoryList = System.Collections.Generic.List<Catalog.Common.Service.Inventory>;
using ShoppingCartItemList = System.Collections.Generic.List<Catalog.Common.Service.ShoppingCartItem>;

namespace Catalog.Client
{
	sealed class GridElement : VirtualGridCellElement
	{
		public static GridItemCollection GridElements { get; private set; }

		public Action<Int32, GridItem> Callback { get; set; }

		public Int32 ProductID
		{
			get
			{
				if (productCartItems != null)
				{
					if (RowIndex < productCartItems.Count && RowIndex >= 0)
						productId = productCartItems[RowIndex].ProductID;
				}
				else
				{
					if (RowIndex < productInventories.Count && RowIndex >= 0)
						productId = productInventories[RowIndex].ProductID;
				}

				return productId;
			}

			set
			{
				if (value != productId)
					productId = value;
			}
		}

		public GridElement(GridItemCollection gridItems, Int32 index)
			: this(gridItems, null, index)
		{ }

		public GridElement(GridItemCollection gridItems, Action<Int32, GridItem> callback, Int32 index)
			: base()
		{
			GridElements = gridItems;
			this.dataIndex = index;
			this.Callback = callback;
		}

		private void IncreaseValue(Object sender, EventArgs e)
		{
			if (ProductID < 0)
				return;

			Int32.TryParse(this.radButtonTextBoxElement.Text, out int value);

			if (value < Int32.MaxValue)
			{
				var newValue = GridElements.Increment(ProductID);
				WatchChanges(newValue, value);
			}
		}

		private void DecreaseValue(Object sender, EventArgs e)
		{
			if (ProductID < 0)
				return;

			Int32.TryParse(this.radButtonTextBoxElement.Text, out int value);

			if (value > 0)
			{
				var newValue = GridElements.Decrement(ProductID);
				WatchChanges(newValue, value);
			}
		}

		private void WatchChanges(Int32 newValue, Int32 oldValue)
		{
			this.radButtonTextBoxElement.Text = newValue.ToString();
			var inventory = GridElements.Get(ProductID);
			Callback?.Invoke(RowIndex, inventory);
		}

		#region VirtualGridCellElement overrides

		public override void Attach(Int32 data, Object context)
		{
			base.Attach(data, context);
			this.radButtonTextBoxElement.TextChanged += TextBoxButton_TextChanged;
		}

		public override void Detach()
		{
			this.radButtonTextBoxElement.TextChanged -= TextBoxButton_TextChanged;
			base.Detach();
		}

		public override bool IsCompatible(Int32 data, Object context)
		{
			var rowElement = context as VirtualGridRowElement;
			return data == this.dataIndex && rowElement.RowIndex >= 0;
		}

		protected override void CreateChildElements()
		{
			base.CreateChildElements();
			this.radButtonTextBoxElement = new RadButtonTextBoxElement()
			{
				MinSize = new Size(80, 40),
				MaxSize = new Size(100, 60),
				TextAlign = HorizontalAlignment.Center,
				Alignment = ContentAlignment.MiddleCenter,
				Shape = new RoundRectShape(3),
				ShowClearButton = true,
				Tag = 0
			};

			this.radButtonTextBoxElement.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

			this.radMinusButtonElement = new RadImageButtonElement()
			{
				Image = Resources.remove,
				Name = "MinusButton",
				DisplayStyle = DisplayStyle.Image,
				ClickMode = ClickMode.Press,
				ShowBorder = false
			};
			this.radPlusButtonElement = new RadImageButtonElement()
			{
				Image = Resources.add,
				Name = "PlusButton",
				DisplayStyle = DisplayStyle.Image,
				ClickMode = ClickMode.Press,
				ShowBorder = false
			};

			this.radButtonTextBoxElement.LeftButtonItems.Add(this.radMinusButtonElement);
			this.radButtonTextBoxElement.RightButtonItems.Add(this.radPlusButtonElement);

			this.radButtonTextBoxElement.TextBoxItem.TextChanging += TextBoxButton_TextChanging;
			this.radButtonTextBoxElement.TextBoxItem.GotFocus += TextBoxButton_GotFocus;
			this.radButtonTextBoxElement.TextBoxItem.LostFocus += TextBoxItem_LostFocus;
			this.radButtonTextBoxElement.KeyPress += TextBoxButtonElement_KeyPress;
			this.radPlusButtonElement.Click += IncreaseValue;
			this.radMinusButtonElement.Click += DecreaseValue;

			this.Children.Add(this.radButtonTextBoxElement);
		}

		protected override void UpdateInfo(VirtualGridCellValueNeededEventArgs args)
		{
			if (args.Value is Int32 value)
				this.radButtonTextBoxElement.Text = value.ToString();
		}

		protected override SizeF ArrangeOverride(SizeF finalSize)
		{
			SizeF size = base.ArrangeOverride(finalSize);
			this.radButtonTextBoxElement.Arrange(new RectangleF(new PointF(0, 0), size));
			return size;
		}

		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			this.radButtonTextBoxElement = null;
			this.radPlusButtonElement = null;
			this.radMinusButtonElement = null;
		}

		#endregion

		#region Events

		private void TextBoxButton_GotFocus(Object sender, EventArgs e)
		{
			textBoxItemHasFocus = true;
		}

		private void TextBoxItem_LostFocus(Object sender, EventArgs e)
		{
			if (radButtonTextBoxElement.Text == String.Empty)
				radButtonTextBoxElement.Text = "0";
			textBoxItemHasFocus = false;
		}

		private void TextBoxButton_TextChanging(Object sender, EventArgs e)
		{
			if (ProductID < 0)
				return;

			Int32.TryParse(this.radButtonTextBoxElement.Text, out Int32 value);

			if (0 <= value && value <= Int32.MaxValue)
			{
				var newValue = GridElements.Change(ProductID, value);
				var inventory = GridElements.Get(ProductID);

				if (textBoxItemHasFocus)
				{
					Callback?.Invoke(RowIndex, inventory);
				}
			}
		}

		private void TextBoxButton_TextChanged(Object sender, EventArgs e)
		{
			Int32.TryParse(this.radButtonTextBoxElement.Text, out Int32 value);
			this.TableElement.GridElement.SetCellValue(value, RowIndex, ColumnIndex, ViewInfo);
		}

		private void TextBoxButtonElement_KeyPress(Object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		#endregion

		#region Fields

		public InventoryList productInventories;
		public ShoppingCartItemList productCartItems = null;
		public Boolean textBoxItemHasFocus;

		public RadButtonTextBoxElement radButtonTextBoxElement;
		public RadImageButtonElement radMinusButtonElement;
		public RadImageButtonElement radPlusButtonElement;

		private Int32 dataIndex = 0;
		private Int32 productId = -1;

		#endregion
	}
}

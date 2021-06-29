using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Catalog.Client.Properties;
using Catalog.Common.Service;

using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
	public class ValueChangedEventArgs : EventArgs
	{
		public int DataId { get; set; }

		public int OldValue { get; set; }

		public int NewValue { get; set; }

		public ValueChangedEventArgs(int dataId, int oldValue, int newValue)
		{
			this.DataId = dataId;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}
	}

	sealed class QuantityCellElement : VirtualGridCellElement
	{
		public RadButtonTextBoxElement radButtonTextBoxElement;

		public RadImageButtonElement radMinusButtonElement;

		public RadImageButtonElement radPlusButtonElement;

		public bool textBoxItemHasFocus;

		private int dataIndex = 0;

		private int productId = -1;

		public Action<int, Inventory> Callback = null;

		public static InventoryCollection Inventories { get; private set; }

		public int ProductID
		{
			get
			{
				if (CartItems != null)
				{
					if (RowIndex < CartItems.Count && RowIndex >= 0)
						productId = CartItems[RowIndex].ProductID;
				}
				else
				{
					if (RowIndex < ProductInventories.Count && RowIndex >= 0)
						productId = ProductInventories[RowIndex].ProductID;
				}

				return productId;
			}

			set
			{
				if (value != productId)
					productId = value;
			}
		}

		public List<ProductInventory> ProductInventories { get; set; }

		public List<ShoppingCartItem> CartItems { get; set; } = null;

		public QuantityCellElement(InventoryCollection productQuantities, int index)
			: this(productQuantities, null, index)
		{
		}

		public QuantityCellElement(
			InventoryCollection productQuantities,
			Action<int, Inventory> callback,
			int index
		) : base()
		{
			Inventories = productQuantities;
			this.dataIndex = index;
			this.Callback = callback;
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

		public override void Attach(int data, object context)
		{
			base.Attach(data, context);
			this.radButtonTextBoxElement.TextChanged += TextBoxButton_TextChanged;
		}

		public override void Detach()
		{
			this.radButtonTextBoxElement.TextChanged -= TextBoxButton_TextChanged;
			base.Detach();
		}

		private void IncreaseValue(Object sender, EventArgs e)
		{
			if (ProductID < 0)
				return;

			Int32.TryParse(this.radButtonTextBoxElement.Text, out int value);

			if (value < Int32.MaxValue)
			{
				var newValue = Inventories.Increment(ProductID);
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
				var newValue = Inventories.Decrement(ProductID);
				WatchChanges(newValue, value);
			}
		}

		private void TextBoxButton_TextChanging(Object sender, EventArgs e)
		{
			if (ProductID < 0)
				return;

			Int32.TryParse(this.radButtonTextBoxElement.Text, out int value);

			if (0 <= value && value <= Int32.MaxValue)
			{
				var newValue = Inventories.Change(ProductID, value);
				var inventory = Inventories.Get(ProductID);

				if (textBoxItemHasFocus)
				{
					Callback?.Invoke(RowIndex, inventory);
				}
			}
		}

		private void WatchChanges(int newValue, int oldValue)
		{
			this.radButtonTextBoxElement.Text = newValue.ToString();
			var inventory = Inventories.Get(ProductID);
			Callback?.Invoke(RowIndex, inventory);
		}

		private void TextBoxButton_TextChanged(Object sender, EventArgs e)
		{
			Int32.TryParse(this.radButtonTextBoxElement.Text, out int value);
			this.TableElement.GridElement.SetCellValue(value, RowIndex, ColumnIndex, ViewInfo);
		}

		private void TextBoxButtonElement_KeyPress(Object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		protected override void UpdateInfo(VirtualGridCellValueNeededEventArgs args)
		{
			if (args.Value is int value)
				this.radButtonTextBoxElement.Text = value.ToString();
		}

		public override bool IsCompatible(int data, object context)
		{
			var rowElement = context as VirtualGridRowElement;
			return data == this.dataIndex && rowElement.RowIndex >= 0;
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
	}

}

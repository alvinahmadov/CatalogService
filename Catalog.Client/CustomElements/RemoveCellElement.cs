using System;
using System.Drawing;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;

using Catalog.Client.Properties;
using System.Diagnostics;

namespace Catalog.Client
{
	class RemoveCellElement : VirtualGridCellElement
	{
		public event EventHandler<Common.GridValueChangedEventArgs> ValueChanged;

		public Action<int> Callback { get; set; } = null;

		public RadButtonElement RemoveButtonElement { get; private set; }

		public RemoveCellElement(int dataIndex)
		{
			this.dataIndex = dataIndex;
			RemoveButtonElement.Click += RemoveButtonElement_Click;
		}

		private void RemoveButtonElement_Click(Object sender, EventArgs e)
		{
			try
			{
				ValueChanged?.Invoke(sender, new Common.GridValueChangedEventArgs(RowIndex, 0, 0));
				Callback?.Invoke(RowIndex);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception: {ex.Message}\n At row: {RowIndex}\nStack: {ex.StackTrace}");
			}
		}

		#region VirtualGridCellElement overrides

		protected override void CreateChildElements()
		{
			base.CreateChildElements();

			this.RemoveButtonElement = new RadButtonElement
			{
				DisplayStyle = DisplayStyle.Image,
				Margin = new Padding(0),
				Padding = new Padding(0),
				ImageAlignment = ContentAlignment.MiddleCenter,
				Alignment = ContentAlignment.MiddleCenter,
				Text = String.Empty,
				HighlightColor = Color.Transparent,
				EnableHighlight = false,
				EnableBorderHighlight = false,
				EnableFocusBorder = false,
				EnableRippleAnimation = false,
				EnableElementShadow = false,
				ShadowColor = Color.Transparent,
				BackColor = Color.Transparent,
				Shape = new CircleShape(),
				Image = Resources.delete
			};
			this.RemoveButtonElement.MaxSize = new Size(40, 40);
			this.RemoveButtonElement.ImagePrimitive.ImageLayout = ImageLayout.Center;
			this.Children.Add(this.RemoveButtonElement);
		}

		public override bool IsCompatible(int data, object context)
		{
			var rowElement = context as VirtualGridRowElement;
			return data == this.dataIndex && rowElement.RowIndex >= 0;
		}

		protected override SizeF ArrangeOverride(SizeF finalSize)
		{
			var size = new SizeF(finalSize.Width - 20, finalSize.Height);
			return base.ArrangeOverride(size);
		}

		protected override Type ThemeEffectiveType => typeof(VirtualGridCellElement);

		protected override void DisposeManagedResources()
		{
			RemoveButtonElement.Dispose();
			base.DisposeManagedResources();
		}

		#endregion

		private int dataIndex;
	}
}

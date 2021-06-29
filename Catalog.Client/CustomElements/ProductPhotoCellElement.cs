using System;
using System.Drawing;
using System.IO;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
	class ProductPhotoCellElement : VirtualGridCellElement
	{
		public ProductPhotoCellElement()
		{ }

		protected override void UpdateInfo(VirtualGridCellValueNeededEventArgs args)
		{
			base.UpdateInfo(args);
		}
		public override bool IsCompatible(int data, object context)
		{
			var rowElement = context as VirtualGridRowElement;
			return data == 0 && rowElement.RowIndex >= 0;
		}
		protected override Type ThemeEffectiveType
		{
			get
			{
				return typeof(VirtualGridCellElement);
			}
		}

		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
		}
		public override bool CanEdit
		{
			get
			{
				return false;
			}
		}
	}
}

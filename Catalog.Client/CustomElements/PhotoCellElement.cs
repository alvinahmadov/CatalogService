using System;

using Telerik.WinControls.UI;

namespace Catalog.Client
{
	class PhotoCellElement : VirtualGridCellElement
	{
		public PhotoCellElement()
		{ }

		protected override Type ThemeEffectiveType => typeof(VirtualGridCellElement);

		public override bool CanEdit => false;

		protected override void UpdateInfo(VirtualGridCellValueNeededEventArgs args)
		{
			base.UpdateInfo(args);
		}
		public override bool IsCompatible(int data, object context)
		{
			var rowElement = context as VirtualGridRowElement;
			return data == 0 && rowElement.RowIndex >= 0;
		}

		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
		}
	}
}

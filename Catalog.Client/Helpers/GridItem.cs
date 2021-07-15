using System;

using Catalog.Common.Service;

namespace Catalog.Client
{
	public class GridItem
	{
		public static ShoppingCart ShoppingCart { get; set; }

		public ShoppingCartItem ShoppingCartItem { get; set; }

		public Int32 ProductID { get; private set; }

		public decimal UnitPrice { get; private set; }

		public Int32 Quantity
		{
			get => ShoppingCartItem.Quantity;
			set
			{
				if (value >= 0)
					ShoppingCartItem.Quantity = value;
			}
		}

		public GridItem(Int32 productID, Decimal unitPrice)
		{
			this.ProductID = productID;
			this.UnitPrice = unitPrice;
		}

		public void UpdateAndSave()
		{
			Update(false);
		}

		public void UpdateAndSaveAsync()
		{
			Update(true);
		}

		private void Update(bool async)
		{
			ShoppingCartItem.ModifiedDate = DateTime.Now;
			ShoppingCart.ModifiedDate = DateTime.Now;
			Entity.Commit(async);
		}
	}
}
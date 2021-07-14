using System;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ShoppingCartItem : EntityModel
	{
		public ShoppingCartItem(ShoppingCart cart, Product product)
		{
			ID = product.ID;
			ProductID = product.ProductID;
			UnitPrice = product.Price;
			ShoppingCart = cart;
			Quantity = 0;
			rowguid = Guid.NewGuid();
			DateCreated = DateTime.Now;
			ModifiedDate = DateTime.Now;
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
				{
					Repository.Repository.Context.ShoppingCartItems.Add(this);
				}

				if (commit)
				{
					Debug.WriteLine("Commit at ShoppingCartItems.Save");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(object entity, bool save = false)
		{
			var cartItem = entity as ShoppingCartItem;

			if (this.Quantity != cartItem.Quantity && this.UnitPrice != cartItem.UnitPrice)
			{
				this.Quantity = cartItem.Quantity;
				this.UnitPrice = cartItem.UnitPrice;
				this.ModifiedDate = DateTime.Now;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ShoppingCartItems.Remove(this);
			Save(false, true);
		}
	}
}
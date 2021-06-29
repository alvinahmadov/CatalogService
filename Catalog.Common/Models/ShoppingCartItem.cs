using System;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ShoppingCartItem : EntityModel
	{
		public ShoppingCartItem(ShoppingCart cart, Product product)
		{
			ID = product.ID;
			ShoppingCart = cart;
			ProductID = product.ProductID;
			Quantity = 0;
			UnitPrice = product.Price;
			Product = product;
			rowguid = Guid.NewGuid();
			DateCreated = DateTime.Now;
			ModifiedDate = DateTime.Now;
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
					Repository.Repository.Context.ShoppingCartItems.Add(this);

				if (commit)
					Commit();
			}
			catch { }
		}

		public override void Update(object entity, bool save = false)
		{
			var cartItem = entity as ShoppingCartItem;

			if (this.Quantity != cartItem.Quantity && this.UnitPrice != cartItem.UnitPrice)
			{
				this.Quantity = cartItem.Quantity;
				this.UnitPrice = cartItem.UnitPrice;
				this.ModifiedDate = DateTime.Now;

				if(save)
					Save(!save, save);
			}

		}

		public override void Delete()
		{
			Repository.Repository.Context.ShoppingCartItems.Remove(this);
			Save(false, true);
		}
	}
}
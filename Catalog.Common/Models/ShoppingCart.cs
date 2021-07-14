using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Catalog.Common.Service
{
	public partial class ShoppingCart : EntityModel
	{
		public ShoppingCart(ShoppingCartStatus shoppingCartStatus)
		{
			this.Status = (byte)shoppingCartStatus;
			this.TotalQuantity = 0;
			this.TotalPrice = 0m;
			this.DateCreated = DateTime.Now;
			this.ModifiedDate = DateTime.Now;
			this.ShoppingCartItems = new HashSet<ShoppingCartItem>();
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
				{
					Repository.Repository.Context.ShoppingCarts.Add(this);
				}


				if (commit)
				{
					Debug.WriteLine("Commit at ShoppingCart.Save");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(object entity, bool commit)
		{
			var cart = entity as ShoppingCart;

			if (this.Status != cart.Status &&
				this.TotalPrice != cart.TotalPrice &&
				this.TotalQuantity != cart.TotalQuantity
			)
			{
				this.Status = cart.Status;
				this.TotalPrice = cart.TotalPrice;
				this.TotalQuantity = cart.TotalQuantity;
				this.ShoppingCartItems = cart.ShoppingCartItems;
				this.ModifiedDate = DateTime.Now;
				if (commit)
					Save(!commit, commit);

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ShoppingCarts.Remove(this);
			Save(false, true);
		}
	}
}
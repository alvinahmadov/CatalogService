using System;
using System.Collections.Generic;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ShoppingCart : Entity
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

		public static void SaveRange(List<ShoppingCart> range, bool commit = true)
		{
			Repository.Repository.Context.ShoppingCarts.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(Boolean commit = false)
		{
			Repository.Repository.Context.ShoppingCarts.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity == null)
				return false;

			var other = entity as ShoppingCart;

			if (!Equals(other))
			{
				this.Status = other.Status;
				this.TotalQuantity = other.TotalQuantity;
				this.TotalPrice = other.TotalPrice;
				this.ShoppingCartItems = other.ShoppingCartItems;
				this.ModifiedDate = DateTime.Now;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ShoppingCarts.Remove(this);
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			var entity = obj as ShoppingCart;

			var equals = (this.Status == entity.Status) &&
						(this.TotalQuantity == entity.TotalQuantity) &&
						(this.TotalPrice == entity.TotalPrice);

			return equals;
		}

		public override Int32 GetHashCode()
		{
			return this.ShoppingCartID.GetHashCode()
			^ this.TotalPrice.GetHashCode() 
			^ this.TotalQuantity.GetHashCode();
		}

		public override String ToString()
		{
			return $"ShoppingCart<[" +
			$"Status: {this.Status}, " +
			$"TotalQuantity: {this.TotalQuantity}, " +
			$"TotalPrice: {this.TotalPrice}, " +
			$"DateCreated: {this.DateCreated}, " +
			$"ModifiedDate: {this.ModifiedDate}" +
			$"]>";
		}
	}
}
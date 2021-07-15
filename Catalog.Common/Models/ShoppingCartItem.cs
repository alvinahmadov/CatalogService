using System;
using System.Collections.Generic;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ShoppingCartItem : Entity
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

		public static void SaveRange(List<ShoppingCartItem> range, Boolean commit = true)
		{
			Repository.Repository.Context.ShoppingCartItems.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(bool commit = false)
		{
			Repository.Repository.Context.ShoppingCartItems.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity == null)
				return false;

			var other = entity as ShoppingCartItem;

			if (!Equals(other))
			{
				this.Quantity = other.Quantity;
				this.UnitPrice = other.UnitPrice;
				this.ModifiedDate = DateTime.Now;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ShoppingCartItems.Remove(this);
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			var entity = obj as ShoppingCartItem;

			var equal = (this.ProductID == entity.ProductID)
					 && (this.Quantity == entity.Quantity)
					 && (this.UnitPrice == entity.UnitPrice);
			return equal;
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override String ToString()
		{
			return $"ShoppingCartItem ({this.ID}) <[" +
			$"ProductID: {this.ProductID}, " +
			$"Quantity: {this.Quantity}, " +
			$"UnitPrice: {this.UnitPrice}, " +
			$"DateCreated: {this.DateCreated}, " +
			$"ModifiedDate: {this.ModifiedDate}" +
			$"]>";
		}
	}
}
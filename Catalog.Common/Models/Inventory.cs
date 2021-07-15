using System;
using System.Collections.Generic;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class Inventory : Entity
	{
		public Inventory(Product product, StockLevel stockLevel, String pack)
		{
			this.ID = product.ID;
			this.ProductID = product.ProductID;
			this.CategoryID = product.Subcategory.CategoryID;
			this.SubcategoryID = product.SubcategoryID;
			this.StockLevel1 = stockLevel.First;
			this.StockLevel2 = stockLevel.Second;
			this.Pack = pack;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Product = product;
		}

		public static void SaveRange(List<Inventory> range, bool commit = true)
		{
			Repository.Repository.Context.Inventories.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(Boolean commit = false)
		{
			Repository.Repository.Context.Inventories.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity is null)
				return false;

			var other = entity as Inventory;

			if (!Equals(other))
			{
				this.ProductID = other.ProductID;
				this.CategoryID = other.CategoryID;
				this.StockLevel1 = other.StockLevel1;
				this.StockLevel2 = other.StockLevel2;
				this.Pack = other.Pack;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.Inventories.Remove(this);
			Commit();
		}

		public override void Cancel()
		{
			if (this.Product != null && this.Product.ProductID != this.ProductID)
			{
				this.ProductID = this.Product.ProductID;
			}
		}

		public override Int32 GetHashCode()
		{
			return this.ProductID.GetHashCode()
			^ this.StockLevel1.GetHashCode()
			^ this.StockLevel2.GetHashCode();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as Inventory;
			return (this.ProductID == other.ProductID)
				&& (this.Product.ProductID == other.Product.ProductID)
				&& (this.StockLevel1 == other.StockLevel1)
				&& (this.StockLevel2 == other.StockLevel2)
				&& (this.Product == other.Product)
				&& (this.Pack == other.Pack);
		}

		public override String ToString()
		{
			return $"ProductInventory <[" +
			$"ProductID: {this.ProductID}, " +
			$"ArticleNumber: {this.Product.ArticleNumber}, " +
			$"StockLevel1: {this.StockLevel1}, " +
			$"StockLevel2: {this.StockLevel2}, " +
			$"Pack: {this.Pack}" +
			$"]>";
		}
	}
}
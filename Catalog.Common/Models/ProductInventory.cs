using System;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ProductInventory : EntityModel
	{
		public ProductInventory(Product product, StockLevel stockLevel, String pack)
		{
			this.ID = product.ID;
			this.ProductID = product.ProductID;
			this.ProductCategoryID = product.ProductSubcategory.ProductCategoryID;
			this.ProductSubcategoryID = product.ProductSubcategoryID;
			this.StockLevel1 = stockLevel.First;
			this.StockLevel2 = stockLevel.Second;
			this.Pack = pack;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Product = product;
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			if (isAddingItem)
			{
				Repository.Repository.Context.ProductInventories.Add(this);
			}

			if (commit)
			{
				Debug.WriteLine("Commit at ProductInventory.Save");
				Commit();
			}
		}

		public override bool Update(object entity, bool save = false)
		{
			var other = entity as ProductInventory;

			if (!Equals(other))
			{
				this.ProductID = other.ProductID;
				this.ProductCategoryID = other.ProductCategoryID;
				this.StockLevel1 = other.StockLevel1;
				this.StockLevel2 = other.StockLevel2;
				this.Pack = other.Pack;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ProductInventories.Remove(this);
			Save(false, true);
		}

		public override void Cancel()
		{
			if (this.Product != null && this.Product.ProductID != this.ProductID)
			{
				this.ProductID = this.Product.ProductID;
			}
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

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as ProductInventory;
			return (this.ProductID == other.ProductID)
			&& (this.Product.ProductID == other.Product.ProductID)
			&& (this.StockLevel1 == other.StockLevel1)
			&& (this.StockLevel2 == other.StockLevel2)
			&& (this.Product == other.Product)
			&& (this.Pack == other.Pack);
		}
	}
}
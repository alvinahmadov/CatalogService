using System;

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
				Repository.Repository.Context.ProductInventories.Add(this);

			if (commit)
				Commit();
		}

		public override void Update(object entity, bool save = false)
		{
			var other = entity as ProductInventory;

			if (this.ProductID != other.ProductID
				&& this.ProductCategoryID != other.ProductCategoryID
				&& this.ProductSubcategoryID != other.ProductSubcategoryID
				&& this.StockLevel1 != other.StockLevel1
				&& this.StockLevel2 != other.StockLevel2
				&& this.Pack != other.Pack)
			{
				this.ProductID = other.ProductID;
				this.ProductCategoryID = other.ProductCategoryID;
				this.StockLevel1 = other.StockLevel1;
				this.StockLevel2 = other.StockLevel2;
				this.Pack = other.Pack;

				if (save)
					Save(!save, save);
			}
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
	}
}
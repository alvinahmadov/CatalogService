using System;
using System.Collections.Generic;

namespace Catalog.Common.Service
{
	public partial class ProductSubcategory : EntityModel
	{
		public ProductSubcategory(int categoryId, string name)
		{
			this.ProductCategoryID = categoryId;
			this.Name = name;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Products = new HashSet<Product>();
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
					Repository.Repository.Context.ProductSubcategories.Add(this);

				if (commit)
					Commit();
			}
			catch { }
		}

		public override void Update(object entity, bool commit = false)
		{
			var category = entity as Product;

			if (this.Name != category.Name)
			{
				this.Name = category.Name;
				this.ModifiedDate = DateTime.Now;
				if (commit)
					Save(!commit, commit);
			}

		}

		public override void Delete()
		{
			Repository.Repository.Context.ProductSubcategories.Remove(this);
			Commit();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class ProductCategory : EntityModel
	{
		public ProductCategory(string name)
		{
			this.Name = name;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.ProductSubcategories = new HashSet<ProductSubcategory>();
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
				{
					Repository.Repository.Context.ProductCategories.Add(this);
				}

				if (commit)
				{
					Debug.WriteLine("Commit at ProductCategory.Save");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(object entity, bool save)
		{
			var category = entity as ProductCategory;

			if (this.Name != category.Name)
			{
				this.Name = category.Name;
				this.ProductSubcategories = category.ProductSubcategories;
				this.ModifiedDate = DateTime.Now;
				if (save)
					Save(false);

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ProductCategories.Remove(this);
			Save(false, true);
		}
	}
}
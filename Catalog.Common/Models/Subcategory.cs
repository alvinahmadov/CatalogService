using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Catalog.Common.Service
{
	public partial class Subcategory : Entity
	{
		public Subcategory(Int32 categoryId, String name)
		{
			this.CategoryID = categoryId;
			this.Name = name;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Products = new HashSet<Product>();
		}

		public static void SaveRange(List<Subcategory> range, Boolean commit = true)
		{
			Repository.Repository.Context.Subcategories.AddRange(range);
			Commit();
		}

		public override void Save(Boolean commit = false)
		{
			Repository.Repository.Context.Subcategories.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity is null)
				return false;

			var other = entity as Subcategory;

			if (!Equals(other))
			{
				this.CategoryID = other.CategoryID;
				this.Name = other.Name;
				this.ModifiedDate = DateTime.Now;
				return true;
			}

			return false;

		}

		public override void Delete()
		{
			Repository.Repository.Context.Subcategories.Remove(this);
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj is null)
				return false;

			var other = obj as Subcategory;

			return (this.Name == other.Name)
				&& (this.CategoryID == other.CategoryID)
				&& (this.ChildSubcategoryID == other.ChildSubcategoryID);
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override String ToString()
		{
			return $"Subcategory ({this.SubcategoryID}) <[" +
			$"Name: {this.Name}, " +
			$"Category Name: {this.Category?.Name}, " +
			$"]>";
		}
	}
}
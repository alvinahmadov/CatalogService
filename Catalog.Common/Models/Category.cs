using System;
using System.Collections.Generic;

namespace Catalog.Common.Service
{
	public partial class Category : Entity
	{
		public Category(String name)
		{
			this.Name = name;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Subcategories = new HashSet<Subcategory>();
		}

		public static void SaveRange(List<Category> range, bool commit = true)
		{
			Repository.Repository.Context.Categories.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(bool commit = false)
		{
			Repository.Repository.Context.Categories.Add(this);

			if (commit)
				Commit();
		}

		public override bool Update(in Object entity)
		{
			if (entity is null)
				return false;

			var other = entity as Category;

			if (!Equals(other))
			{
				this.Name = other.Name;
				this.Subcategories = other.Subcategories;
				this.ModifiedDate = DateTime.Now;
				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.Categories.Remove(this);
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj is null)
				return false;

			var other = obj as Category;

			var equals = this.Name == other.Name;

			return equals;
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override String ToString()
		{
			return $"Product ({this.CategoryID}) <[" +
			$"Name: {this.Name}, " +
			$"Subcategories: {this.Subcategories.Count}, " +
			$"]>";
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class Product : EntityModel
	{
		public Product(
			int productId,
			string articleNumber,
			string code,
			string brand,
			string name,
			string description,
			decimal price
		)
		{
			this.ProductID = productId;
			this.ArticleNumber = articleNumber;
			this.Code = code;
			this.Brand = brand;
			this.Name = name;
			this.Price = price;
			this.Description = description;
			this.rowguid = Guid.NewGuid();
			this.ModifiedDate = DateTime.Now;
			this.Description = "";
			this.SpecialOfferProducts = new HashSet<SpecialOfferProduct>();
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
				{
					Repository.Repository.Context.Products.Add(this);
				}

				if (commit)
				{
					Debug.WriteLine("Commit at Product.Save");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(object entity, bool commit = false)
		{
			var other = entity as Product;

			if (!Equals(other))
			{
				this.ProductID = other.ProductID;
				this.ArticleNumber = other.ArticleNumber;
				this.Code = other.Code;
				this.Brand = other.Brand;
				this.Name = other.Name;
				this.Description = other.Description;
				this.Price = other.Price;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.Products.Remove(this);
			Save(false, true);
		}

		public override String ToString()
		{
			return $"Product <[" +
			$"ProductID: {this.ProductID}, " +
			$"ArticleNumber: {this.ArticleNumber}, " +
			$"Code: {this.Code}, Brand: {this.Brand}, " +
			$"Name: {this.Name}, Price: {this.Price}" +
			$"]>";
		}

		public override Boolean Equals(Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as Product;

			return (this.ProductID == other.ProductID)
			&& (this.ArticleNumber == other.ArticleNumber)
			&& (this.Code == other.Code)
			&& (this.Brand == other.Brand)
			&& (this.Name == other.Name)
			&& (this.Description == other.Description)
			&& (this.Price == other.Price);
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class Product : Entity
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

		public static void SaveRange(List<Product> range, Boolean commit = true)
		{
			Repository.Repository.Context.Products.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(Boolean commit = false)
		{
			Repository.Repository.Context.Products.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity is null)
				return false;

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
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj is null)
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
			return this.ProductID.GetHashCode()
			^ this.ArticleNumber.GetHashCode()
			^ this.Code.GetHashCode();
		}

		public override String ToString()
		{
			return $"Product ({this.ID}) <[" +
			$"ProductID: {this.ProductID}, " +
			$"ArticleNumber: {this.ArticleNumber}, " +
			$"Code: {this.Code}, " +
			$"Brand: {this.Brand}, " +
			$"Name: {this.Name}, " +
			$"Price: {this.Price}" +
			$"]>";
		}
	}
}
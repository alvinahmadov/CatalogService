using System;
using System.Collections.Generic;

using Catalog.Common.Repository;

namespace Catalog.Common.Service
{
	public partial class Product : EntityModel
	{
		public Product(
			int id,
			string articleNumber,
			string code,
			string brand,
			string name,
			string description,
			decimal price
		)
		{
			this.ProductID = id;
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
					Repository.Repository.Context.Products.Add(this);

				if (commit)
					Commit();
			}
			catch { }
		}

		public override void Update(object entity, bool commit = false)
		{
			var product = entity as Product;

			if (this.ProductID != product.ProductID &&
				this.ArticleNumber != product.ArticleNumber &&
				this.Code != product.Code &&
				this.Brand != product.Brand &&
				this.Name != product.Name &&
				this.Description != product.Description &&
				this.Price.CompareTo(product.Price) != 0 &&
				this.ProductSubcategoryID != product.ProductSubcategoryID
			)
			{
				this.ProductID = product.ProductID;
				this.ArticleNumber = product.ArticleNumber;
				this.Code = product.Code;
				this.Brand = product.Brand;
				this.Name = product.Name;
				this.Description = product.Description;
				this.Price = product.Price;
				this.ProductSubcategoryID = product.ProductSubcategoryID;
				if (commit)
					Save(!commit, commit);
			}

		}

		public override void Delete()
		{
			Repository.Repository.Context.Products.Remove(this);
			Commit();
		}
	}
}
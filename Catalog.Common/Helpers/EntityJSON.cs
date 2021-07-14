using System;

using Catalog.Common.Service;

using JsonObject = Newtonsoft.Json.Linq.JObject;

namespace Catalog.Common
{
	public interface EntityJSON<TEntity>
	{
		TEntity AsEntity();
	}

	public struct CategoryJSON : EntityJSON<ProductCategory>
	{
		public string Name => this.data[API.Category.NAME].ToObject<String>();

		public CategoryJSON(JsonObject data)
		{
			this.data = data;
		}

		public ProductCategory AsEntity()
		{
			return new ProductCategory(Name);
		}

		private JsonObject data;
	}

	public struct ProductJSON : EntityJSON<Product>
	{
		#region Properties 

		public Int32 Id => this.data[API.Product.ID].ToObject<Int32>();

		public String Name => this.data[API.Product.NAME].ToObject<String>();

		public String ArticleNumber => this.data[API.Product.ARTICLE_NUMBER].ToObject<String>();

		public String Code => this.data[API.Product.CODE].ToObject<String>();

		public String Brand => this.data[API.Product.BRAND].ToObject<String>();

		public Decimal Price => Converter.ToDecimal(data[API.Product.PRICE].ToObject<String>());

		public String Image => this.data[API.Product.IMAGE].ToObject<String>();

		public String Description => this.data[API.Product.DESCRIPTION].ToObject<String>();

		public String Package => this.data[API.Product.PACKAGE].ToObject<String>();

		public String Stock1 => this.data[API.Product.STOCK1].ToObject<String>();

		public String Stock2 => this.data[API.Product.STOCK2].ToObject<String>();

		public String Sub1 => this.data[API.Product.SUB1].ToObject<String>();

		public String Sub2 => this.data[API.Product.SUB2].ToObject<String>();

		#endregion

		public ProductJSON(JsonObject data)
		{
			this.data = data;
		}

		public Product AsEntity()
		{
			return new Product(Id, ArticleNumber, Code, Brand, Name, Description, Price);
		}

		private JsonObject data;
	}
}
using System;

using Catalog.Common.Service;

using JsonObject = Newtonsoft.Json.Linq.JObject;

namespace Catalog.Common
{
	public interface EntityJSON<TEntity>
	{
		TEntity AsEntity();
	}

	public struct CategoryJSON : EntityJSON<Category>
	{
		public string Name => this.data[API.Category.NAME].ToObject<String>();

		public CategoryJSON(JsonObject data)
		{
			this.data = data;
		}

		public Category AsEntity()
		{
			return new Category(Name);
		}

		private JsonObject data;
	}

	public struct ProductJSON : EntityJSON<Product>
	{
		#region Properties 

		public Int32 Id => Data[API.Product.ID].ToObject<Int32>();

		public String Name => Data[API.Product.NAME].ToObject<String>();

		public String ArticleNumber => Data[API.Product.ARTICLE_NUMBER].ToObject<String>();

		public String Code => Data[API.Product.CODE].ToObject<String>();

		public String Brand => Data[API.Product.BRAND].ToObject<String>();

		public Decimal Price => Converter.ToDecimal(Data[API.Product.PRICE].ToObject<String>());

		public String Image => Data[API.Product.IMAGE].ToObject<String>();

		public String Description => Data[API.Product.DESCRIPTION].ToObject<String>();

		public String Package => Data[API.Product.PACKAGE].ToObject<String>();

		public String Stock1 => Data[API.Product.STOCK1].ToObject<String>();

		public String Stock2 => Data[API.Product.STOCK2].ToObject<String>();

		public String Sub1 => Data[API.Product.SUB1].ToObject<String>();

		public String Sub2 => Data[API.Product.SUB2].ToObject<String>();

		public JsonObject Data => this.data;

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
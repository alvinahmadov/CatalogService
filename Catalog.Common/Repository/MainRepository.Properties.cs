using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public enum CacheType
	{
		ALL,
		PRODUCT,
		INVENTORY,
		CATEGORY,
		SUBCATEGORY,
		PHOTO
	}

	public partial class MainRepository
	{
		#region Fields

		private static List<Product> productsCache = null;
		private static List<ProductCategory> productCategoriesCache = null;
		private static List<ProductSubcategory> productSubcategoriesCache = null;
		private static List<ProductInventory> productInventoryCache = null;
		private static List<ProductPhoto> productPhotoCache = null;

		#endregion

		public static void ResetCache(CacheType type = CacheType.ALL)
		{
			switch (type)
			{
				case CacheType.PRODUCT:
					productsCache = null;
					break;
				case CacheType.INVENTORY:
					productInventoryCache = null;
					break;
				case CacheType.CATEGORY:
					productCategoriesCache = null;
					break;
				case CacheType.SUBCATEGORY:
					productSubcategoriesCache = null;
					break;
				case CacheType.PHOTO:
					productPhotoCache = null;
					break;
				default:
					productsCache = null;
					productCategoriesCache = null;
					productSubcategoriesCache = null;
					productInventoryCache = null;
					productPhotoCache = null;
					break;
			}
		}

		#region Properties

		public static List<Product> ProductsCache
		{
			get
			{
				if (productsCache == null)
				{
					productsCache = Context.Products
											.Include(p => p.ProductSubcategory)
											.ToList();
				}

				return productsCache;
			}
			set
			{
				if (productsCache != value)
					productsCache = value;
			}
		}

		public static List<ProductInventory> ProductInventoriesCache
		{
			get
			{
				if (productInventoryCache == null)
					productInventoryCache = Context.ProductInventories
															.Include(p => p.Product)
															.ToList();

				return productInventoryCache;
			}
			set
			{
				if (productInventoryCache != value)
				{
					productInventoryCache = value;
				}
			}
		}

		public static List<ProductCategory> ProductCategoriesCache
		{
			get
			{
				if (productCategoriesCache == null)
					productCategoriesCache = Context.ProductCategories
													.Include(q => q.ProductSubcategories)
													.ToList();

				return productCategoriesCache;
			}
			private set
			{
				if (productCategoriesCache != value)
					productCategoriesCache = value;
			}
		}

		public static List<ProductSubcategory> ProductSubcategoriesCache
		{
			get
			{
				if (productSubcategoriesCache == null)
					productSubcategoriesCache = Context.ProductSubcategories.ToList();
				return productSubcategoriesCache;
			}
			private set
			{
				if (productSubcategoriesCache != value)
					productSubcategoriesCache = value;
			}

		}

		public static List<ProductPhoto> ProductPhotoCache
		{
			get
			{
				if (productPhotoCache == null)
					productPhotoCache = Context.ProductPhotoes.ToList();

				return productPhotoCache;
			}
			private set
			{
				if (productPhotoCache != value)
					productPhotoCache = value;
			}
		}

		#endregion
	}
}

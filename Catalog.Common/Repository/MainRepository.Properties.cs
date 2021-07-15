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
		PHOTO,
		CART_ITEM
	}

	public partial class MainRepository
	{
		#region Fields

		private static List<Product> productsCache = null;
		private static List<Category> categoriesCache = null;
		private static List<Subcategory> subcategoriesCache = null;
		private static List<Inventory> inventoryCache = null;
		private static List<ShoppingCartItem> shoppingCartItemCache = null;
		private static List<Photo> photoCache = null;

		#endregion

		public static void ResetCaches(params CacheType[] cacheTypes) 
		{
			foreach (var cacheType in cacheTypes)
				ResetCache(cacheType);
		}

		public static void ResetCache(CacheType type = CacheType.ALL)
		{
			switch (type)
			{
				case CacheType.PRODUCT:
					productsCache = null;
					break;
				case CacheType.INVENTORY:
					inventoryCache = null;
					break;
				case CacheType.CATEGORY:
					categoriesCache = null;
					break;
				case CacheType.SUBCATEGORY:
					subcategoriesCache = null;
					break;
				case CacheType.PHOTO:
					photoCache = null;
					break;
				case CacheType.CART_ITEM:
					shoppingCartItemCache = null;
					break;
				default:
					productsCache = null;
					categoriesCache = null;
					subcategoriesCache = null;
					inventoryCache = null;
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
											.Include(p => p.Subcategory)
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

		public static List<Inventory> InventoriesCache
		{
			get
			{
				if (inventoryCache == null)
					inventoryCache = Context.Inventories
													.Include(p => p.Product)
													.ToList();

				return inventoryCache;
			}
			set
			{
				if (inventoryCache != value)
				{
					inventoryCache = value;
				}
			}
		}

		public static List<Category> CategoriesCache
		{
			get
			{
				if (categoriesCache == null)
					categoriesCache = Context.Categories
													.Include(q => q.Subcategories)
													.ToList();

				return categoriesCache;
			}
			set
			{
				if (categoriesCache != value)
					categoriesCache = value;
			}
		}

		public static List<Subcategory> SubcategoriesCache
		{
			get
			{
				if (subcategoriesCache == null)
					subcategoriesCache = Context.Subcategories.ToList();
				return subcategoriesCache;
			}
			set
			{
				if (subcategoriesCache != value)
					subcategoriesCache = value;
			}

		}

		public static List<Photo> PhotoCache
		{
			get
			{
				if (photoCache == null)
					photoCache = Context.Photos.ToList();

				return photoCache;
			}
			set
			{
				if (photoCache != value)
					photoCache = value;
			}
		}

		public static List<Photo> Photos
		{
			get => photoCache;
			set
			{
				if (photoCache != value)
					photoCache = value;
			}
		}

		public static List<ShoppingCartItem> ShoppingCartItemsCache
		{
			get
			{
				if (shoppingCartItemCache == null)
					shoppingCartItemCache = Context.ShoppingCartItems.ToList();

				return shoppingCartItemCache;
			}
			set
			{
				if (shoppingCartItemCache != value)
				{
					shoppingCartItemCache = value;
				}
			}
		}

		#endregion
	}
}

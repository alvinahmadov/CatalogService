#pragma warning disable IDE0039 // Use local function
using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Data.Entity;

using Catalog.Common.Service;

using DatabaseSettings = Catalog.Common.Service.Settings;
using Categories = System.Collections.Generic.List<Catalog.Common.Service.Category>;
using Subcategories = System.Collections.Generic.List<Catalog.Common.Service.Subcategory>;
using Products = System.Collections.Generic.List<Catalog.Common.Service.Product>;
using Inventories = System.Collections.Generic.List<Catalog.Common.Service.Inventory>;
using ShoppingCartItems = System.Collections.Generic.List<Catalog.Common.Service.ShoppingCartItem>;
using Photos = System.Collections.Generic.List<Catalog.Common.Service.Photo>;

namespace Catalog.Common.Repository
{
	public partial class WebWorker
	{
		public static void LoadData()
		{
			if (UpdateStatus == UpdateStatus.Started)
				return;

			UpdateStatus = UpdateStatus.Started;

			var settings = InitSettings();
			LoadCategories();
			LoadSubcategories();
			LoadProducts();
			if (settings.LoadImage && !PhotosUpdating)
				System.Threading.Tasks.Task.Run(LoadPhotos);


			MainRepository.ResetCache();
			UpdateStatus = UpdateStatus.Finished;
			OnSuccess();
		}

		private static DatabaseSettings InitSettings()
		{
			var dbSettings = Repository.Context.Settings.SingleOrDefault();

			if (dbSettings == null)
			{
				dbSettings = new DatabaseSettings(1);
				dbSettings.Save(true);
			}

			return dbSettings;
		}

		private static void LoadCategories()
		{
			UpdateStatus = UpdateStatus.Started;
			int count = 0;
			var categories = new Categories();
			var response = RestAPIManager.FetchProductCategories().Result;

			if (response is null)
				return;

			foreach (var data in response)
			{
				var category = new CategoryJSON(data).AsEntity();
				var dbCategory = Repository
								 .Context
								 .Categories
								 .Where(c => c.Name.Equals(category.Name))
								 .SingleOrDefault();

				if (dbCategory == null)
				{
					Log(MESSAGE.Category.ADD_STATUS, category.Name);
					categories.Add(category);
					count++;
				}
			}

			LoadShoppingCart();
			Category.SaveRange(categories);
			MainRepository.ResetCache(CacheType.CATEGORY);
			UpdateStatus = UpdateStatus.Finished;

			if (count == 0)
				count = MainRepository.CategoriesCache.Count;

			Log(MESSAGE.Category.LOADED_STATUS, ContentAlignment.MiddleCenter, count);
		}

		private static void LoadSubcategories()
		{
			int count = 0;
			UpdateStatus = UpdateStatus.Started;

			var subcategories = new Subcategories();
			MainRepository.ResetCache(CacheType.SUBCATEGORY);

			foreach (var category in MainRepository.CategoriesCache)
			{
				var response = RestAPIManager.FetchSubcategories(category.Name).Result;

				if (response == null)
					continue;

				foreach (var data in response)
				{
					Subcategory baseSubcategory = null;
					Subcategory childSubcategory = null;

					for (int index = 1; index <= 2; ++index)
					{
						var key = $"subcategory{index}";

						if (!data.ContainsKey(key))
							continue;

						var subcategoryName = data[key].ToObject<String>();
						var exists = MainRepository.SubcategoriesCache
												   .Exists(s => Base64.Encode(s.Name) == Base64.Encode(subcategoryName));

						if (!exists)
						{
							var subcategory = new Subcategory(category.CategoryID, subcategoryName);
							subcategories.Add(subcategory);
							count++;
							Log(MESSAGE.Subcategory.ADD_STATUS, subcategory.Name);

							if (index == 1)
							{
								baseSubcategory = subcategory;
							}
							else if (index == 2)
							{
								childSubcategory = subcategory;
							}
						}
					}

					if (childSubcategory != null)
					{
						baseSubcategory.ChildSubcategoryID = childSubcategory.SubcategoryID;
					}

				}
			}
			Subcategory.SaveRange(subcategories);
			MainRepository.ResetCache(CacheType.SUBCATEGORY);
			UpdateStatus = UpdateStatus.Finished;

			if (count == 0)
				count = MainRepository.SubcategoriesCache.Count;

			Log(MESSAGE.Subcategory.LOADED_STATUS, ContentAlignment.MiddleCenter, count);
		}

		private static void LoadProducts()
		{
			UpdateStatus = UpdateStatus.Started;
			var cart = LoadShoppingCart();

			var products = new Products();
			var inventories = new Inventories();
			var cartItems = new ShoppingCartItems();

			var responses = RestAPIManager.FetchProducts().Result;
			int index = MainRepository.ProductsCache.Count;
			int createCount = 0;
			int updateCount = 0;
			string subcategory = "";

			foreach (var response in responses)
			{
				if (response is null)
					continue;

				foreach (var data in response)
				{
					var json = new ProductJSON(data);
					var productId = json.Id;
					var product = json.AsEntity();
					var stockLevel = new StockLevel(json.Stock1, json.Stock2);

					SubcategoryHelper.Set(json, in product, out String sub1, out String sub2);

					var dbItem = Repository.Context
										   .Products
										   .Where(p => productId == p.ProductID)
										   .SingleOrDefault();
					if (dbItem == null)
					{
						product.ID = ++index;
						var inventory = new Inventory(product, stockLevel, json.Package);
						var cartItem = new ShoppingCartItem(cart, product);

						products.Add(product);
						inventories.Add(inventory);
						cartItems.Add(cartItem);

						createCount++;
						Log(MESSAGE.Product.ADD_STATUS, product.Name);
					}
					else
					{
						if (dbItem.Update(product))
						{
							UpdateInventory(product, stockLevel, json.Package);
							UpdateShoppingCartItem(product, cart);

							updateCount++;
							Log(MESSAGE.Product.UPD_STATUS, product.Name);
						}
					}

					if (subcategory != sub1 || subcategory != sub2)
					{
						subcategory = sub1;
					}
				}
			}

			Product.SaveRange(products);
			Inventory.SaveRange(inventories);
			ShoppingCartItem.SaveRange(cartItems);

			MainRepository.ResetCaches(CacheType.PRODUCT, CacheType.INVENTORY, CacheType.CART_ITEM);

			UpdateStatus = UpdateStatus.Finished;

			int count = 0;

			if (createCount > 0 && updateCount == 0)
			{
				count = createCount;
				Log(MESSAGE.Product.LOADED_STATUS, ContentAlignment.MiddleCenter, count);
			}
			else if (updateCount > 0 && createCount == 0)
			{
				count = updateCount;
				Log(MESSAGE.Product.UPDATED_STATUS, ContentAlignment.MiddleCenter, count);
			}
			else if (createCount > 0 && updateCount > 0)
			{
				count = updateCount + createCount;
				Log(MESSAGE.Product.MIXED_STATUS2, ContentAlignment.MiddleCenter, createCount, updateCount);
			}
			else
			{
				count = MainRepository.ProductsCache.Count;
				Log(MESSAGE.Product.MIXED_STATUS, ContentAlignment.MiddleCenter, count);
			}
		}

		private static void LoadPhotos()
		{
			if (!HasConnection)
				return;
			int imageCount = 0;

			MainRepository.ResetCache(CacheType.PHOTO);
			MainRepository.Photos = new Photos();
			UpdateStatus = UpdateStatus.Started;

			foreach (var product in MainRepository.ProductsCache)
			{
				var response = RestAPIManager.FetchProduct(product.Code).Result;
				if (response == null)
					continue;

				var data = response[0];
				var photoId = product.ID;
				var fileName = new ProductJSON(data).Image;
				var photo = new Photo(photoId, fileName) { Product = product };
				MainRepository.Photos.Add(photo);

				if (fileName.Length == 0)
					continue;
				PhotosUpdating = true;
				LoadPhoto(photoId, fileName);

				imageCount++;
			}

			Photo.SaveRange(MainRepository.Photos);
			MainRepository.ResetCache(CacheType.PHOTO);

			OnSuccess();
			UpdateStatus = UpdateStatus.Finished;
		}

		private static void LoadPhoto(Int32 photoId, String filename)
		{
			Action<Byte[]> downloadCallback = bytes =>
			{
				using (var memstream = new MemoryStream(bytes))
				{
					var image = Image.FromStream(memstream);
					var thumbnailPhoto = ImageUtil.GetThumbnailPhoto(ref image);
					var largePhoto = ImageUtil.GetLargePhoto(ref image);
					var thumbNailPhotoBytes = ImageUtil.GetThumbnailPhotoBytes(ref thumbnailPhoto);
					var largePhotoBytes = ImageUtil.GetLargePhotoBytes(ref largePhoto);

					var photo = MainRepository.Photos.Find(p => p.ID == photoId);

					photo.ThumbNailPhoto = thumbNailPhotoBytes;
					photo.LargePhoto = largePhotoBytes;

					var dbPhoto = Repository.Context.Photos.Find(photo.ID);

					if (dbPhoto == null)
						Log(MESSAGE.Photo.ADD_STATUS, photo);
					else if (dbPhoto.Update(photo))
						Log(MESSAGE.Photo.UPD_STATUS, photo);
				}

				PhotosUpdating = false;
			};
			
			using (var manager = new DownloadManager(Properties.Settings.Default.imageURL))
			{
				manager.DownloadAsBytes(filename, downloadCallback, out string errorMessage);
			};
		}

		private static ShoppingCart LoadShoppingCart()
		{
			ShoppingCart shoppingCart = Repository
										.Context
										.ShoppingCarts
										.Include(sc => sc.ShoppingCartItems)
										.SingleOrDefault();

			if (shoppingCart == null)
			{
				shoppingCart = new ShoppingCart(ShoppingCartStatus.Pending);
				shoppingCart.Save(true);
			}

			return shoppingCart;
		}

		private static void UpdateInventory(Product product, StockLevel stockLevel, String pack)
		{
			var productInventory = new Inventory(product, stockLevel, pack);
			var dbItem = Repository.Context
									.Inventories
									.Where(pi => product.ProductID == pi.ProductID)
									.SingleOrDefault();
			dbItem?.Update(productInventory);
		}

		private static void UpdateShoppingCartItem(Product product, ShoppingCart shoppingCart)
		{
			var dbItem = Repository.Context
								   .ShoppingCartItems
								   .Where(item => product.ProductID == item.ProductID)
								   .SingleOrDefault();

			var cartItem = new ShoppingCartItem(shoppingCart, product);
			dbItem?.Update(cartItem);
		}

		#region Logging

		private static void Log(String message, params Object[] args)
		{
			Log(message, ContentAlignment.MiddleLeft, args);
		}

		private static void Log(String message, ContentAlignment alignment, params Object[] args)
		{
			var logMessage = String.Format(message, args);
			LoggingCallback?.Invoke(logMessage, alignment);
		}

		private static void OnSuccess() 
		{
			SuccessCallback?.Invoke();
		}

		private static void OnError()
		{
			SuccessCallback?.Invoke();
		}

		#endregion
	}
}
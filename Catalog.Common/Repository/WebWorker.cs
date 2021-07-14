using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;

using Catalog.Common.Service;

namespace Catalog.Common.Repository
{
	public partial class WebWorker
	{
		public static void InitSettings()
		{
			var dbSettings = Repository.Context.Settings.SingleOrDefault();
			if (dbSettings == null)
			{
				dbSettings = new Settings(1);
				dbSettings.Save(true, true);
			}
		}

		public static int InitProductCategories()
		{
			UpdateStatus = UpdateStatus.Started;
			int count = 0;
			var productCategories = new List<ProductCategory>();
			var response = RestAPIManager.FetchProductCategories().Result;

			if (response is null)
				return count;

			foreach (var data in response)
			{
				var category = new CategoryJSON(data).AsEntity();
				var dbCategory = Repository
								 .Context
								 .ProductCategories
								 .Where(c => c.Name.Equals(category.Name))
								 .SingleOrDefault();

				if (dbCategory == null)
				{
					Log(MESSAGE.Category.ADD_STATUS, category.Name);
					productCategories.Add(category);
					count++;
				}
			}

			InitShoppingCart();

			Repository.Context.ProductCategories.AddRange(productCategories);
			EntityModel.Commit();

			MainRepository.ResetCache(CacheType.CATEGORY);

			if (count == 0)
				count = MainRepository.ProductCategoriesCache.Count;

			Log(MESSAGE.Category.LOADED_STATUS, ContentAlignment.MiddleCenter, count);

			UpdateStatus = UpdateStatus.Finished;
			return count;
		}

		public static int InitProductSubcategories()
		{
			int count = 0;
			UpdateStatus = UpdateStatus.Started;

			var productSubcategories = new List<ProductSubcategory>();
			MainRepository.ResetCache(CacheType.SUBCATEGORY);

			foreach (var category in MainRepository.ProductCategoriesCache)
			{
				var response = RestAPIManager.FetchSubcategories(category.Name).Result;

				if (response == null)
					continue;

				foreach (var data in response)
				{
					ProductSubcategory baseSubcategory = null;
					ProductSubcategory childSubcategory = null;

					for (int index = 1; index <= 2; ++index)
					{
						var key = $"subcategory{index}";

						if (!data.ContainsKey(key))
							continue;

						var subcategoryName = data[key].ToObject<String>();
						var exists = MainRepository.ProductSubcategoriesCache
												   .Exists(s => Base64.Encode(s.Name) == Base64.Encode(subcategoryName));

						if (!exists)
						{
							var subcategory = new ProductSubcategory(category.ProductCategoryID, subcategoryName);
							Log(MESSAGE.Subcategory.ADD_STATUS, subcategory.Name);
							productSubcategories.Add(subcategory);
							count++;

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
						baseSubcategory.ChildSubcategoryID = childSubcategory.ProductSubcategoryID;
					}

				}
			}

			Repository.Context.ProductSubcategories.AddRange(productSubcategories);
			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.SUBCATEGORY);

			if (count == 0)
				count = MainRepository.ProductSubcategoriesCache.Count;

			Log(MESSAGE.Subcategory.LOADED_STATUS, ContentAlignment.MiddleCenter, count);
			UpdateStatus = UpdateStatus.Finished;

			return count;
		}

		public static int InitProducts()
		{
			if (!HasConnection)
				return -1;

			UpdateStatus = UpdateStatus.Started;
			var cart = InitShoppingCart();

			var products = new List<Product>();
			var productInventories = new List<ProductInventory>();
			var cartItems = new List<ShoppingCartItem>();

			MainRepository.ResetCaches(CacheType.PRODUCT, CacheType.INVENTORY, CacheType.CART_ITEM);

			var responses = RestAPIManager.FetchProducts().Result;
			int index = MainRepository.ProductsCache.Count;
			int createCount = 0;
			int updateCount = 0;
			string subcategory = "";

			foreach (var response in responses)
			{
				foreach (var data in response)
				{
					var json = new ProductJSON(data);
					var productId = json.Id;

					if (productId >= 200)
						break;

					var product = json.AsEntity();
					var stockLevel = new StockLevel(json.Stock1, json.Stock2);
					var sub1 = json.Sub1;
					var sub2 = data.ContainsKey("subcategory2")
										? json.Sub2
										: String.Empty;

					var dbSubcategory = MainRepository.ProductSubcategoriesCache
													  .Find(c => c.Name.ToLower().Equals(sub1.ToLower()));

					var productSubcategory = dbSubcategory;

					if (!sub2.Equals(String.Empty))
					{
						if (dbSubcategory != null)
						{
							productSubcategory = MainRepository.ProductSubcategoriesCache
												.Where(s => s.ChildSubcategoryID == dbSubcategory.ChildSubcategoryID)
												.FirstOrDefault();
						}
						else
						{
							var sub2Lower = sub2.ToLower();
							dbSubcategory = MainRepository.ProductSubcategoriesCache
														  .Where(s => s.Name.ToLower().Equals(sub2Lower))
														  .FirstOrDefault();
						}

						productSubcategory = dbSubcategory;
					}
					else
						productSubcategory = dbSubcategory;

					product.ProductSubcategoryID = productSubcategory.ProductSubcategoryID;
					product.ProductSubcategory = MainRepository.ProductSubcategoriesCache
															   .Find(c => c.ProductSubcategoryID == product.ProductSubcategoryID);

					var dbItem = Repository.Context
										   .Products
										   .Where(p => productId == p.ProductID)
										   .SingleOrDefault();
					if (dbItem == null)
					{
						product.ID = ++index;
						var productInventory = new ProductInventory(product, stockLevel, json.Package);
						var cartItem = new ShoppingCartItem(cart, product);

						products.Add(product);
						productInventories.Add(productInventory);
						cartItems.Add(cartItem);

						createCount++;

						Log(MESSAGE.Product.ADD_STATUS, product.Name);
					}
					else
					{
						if (dbItem.Update(product))
						{
							UpdateProductInventory(product, stockLevel, json.Package);
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

			Debug.WriteLine($"Products.Count => {products.Count}");
			Debug.WriteLine($"ProductInventories.Count => {productInventories.Count}");
			Debug.WriteLine($"ShoppingCartItems.Count => {cartItems.Count}");

			Repository.Context.Products.AddRange(products);
			Repository.Context.ProductInventories.AddRange(productInventories);
			Repository.Context.ShoppingCartItems.AddRange(cartItems);
			EntityModel.Commit();

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

			return count;
		}

		public static void InitProductPhotos()
		{
			if (!HasConnection)
				return;

			UpdateStatus = UpdateStatus.Started;
			int imageCount = 0;
			var productPhotos = new List<ProductPhoto>();

			foreach (var product in MainRepository.ProductsCache)
			{
				var response = RestAPIManager.FetchProduct(product.Code).Result;
				if (response == null)
					continue;

				var data = response[0];
				var json = new ProductJSON(data);
				var photoId = product.ID;
				var fileName = json.Image;

				if (json.Id >= 200)
					break;

				var photo = new ProductPhoto(photoId, fileName) { Product = product };
				productPhotos.Add(photo);

				if (fileName.Length == 0)
					continue;

				PhotosUpdating = true;
				InitProductPhoto(ref photo);

				imageCount++;
			}

			Repository.Context.ProductPhotos.AddRange(productPhotos);
			EntityModel.Commit();

			MainRepository.ResetCache(CacheType.PHOTO);
			UpdateStatus = UpdateStatus.Finished;
		}

		private static void InitProductPhoto(ref ProductPhoto productPhoto)
		{
			var photo = productPhoto;
			var filename = productPhoto.FileName;

			Action<Byte[]> callback = bytes =>
			{
				using (var memstream = new MemoryStream(bytes))
				{
					var image = Image.FromStream(memstream);
					var thumbnailPhoto = ImageUtil.GetThumbnailPhoto(ref image);
					var largePhoto = ImageUtil.GetLargePhoto(ref image);
					var thumbNailPhotoBytes = ImageUtil.GetThumbnailPhotoBytes(ref thumbnailPhoto);
					var largePhotoBytes = ImageUtil.GetLargePhotoBytes(ref largePhoto);

					photo.ThumbNailPhoto = thumbNailPhotoBytes;
					photo.LargePhoto = largePhotoBytes;

					var dbPhoto = Repository.Context.ProductPhotos.Find(photo.ID);

					if (dbPhoto == null)
						Log(MESSAGE.Photo.ADD_STATUS, photo);
					else if (dbPhoto.Update(photo))
						Log(MESSAGE.Photo.UPD_STATUS, photo);
				}

				PhotosUpdating = false;
			};

			using (var manager = new DownloadManager(Configuration.Get("imageURL")))
			{
				manager.DownloadAsBytes(filename, callback, out string errorMessage);
			};
		}

		private static void UpdateProductInventory(Product product, StockLevel stockLevel, string pack)
		{
			var productInventory = new ProductInventory(product, stockLevel, pack);
			var dbItem = Repository.Context
									.ProductInventories
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

		private static ShoppingCart InitShoppingCart()
		{
			ShoppingCart shoppingCart = Repository
										.Context
										.ShoppingCarts
										.Include(sc => sc.ShoppingCartItems)
										.SingleOrDefault();

			if (shoppingCart == null)
			{
				shoppingCart = new ShoppingCart(ShoppingCartStatus.PENDING);
				shoppingCart.Save();
			}

			return shoppingCart;
		}

		#region Logging

		private static void Log(String message, params object[] args)
		{
			Log(message, ContentAlignment.MiddleLeft, args);
		}

		private static void Log(String message, ContentAlignment alignment, params object[] args)
		{
			var logMessage = String.Format(message, args);
			LoggingCallback?.Invoke(logMessage, alignment);
		}

		#endregion
	}
}
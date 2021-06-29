using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data.Entity;

using JsonObject = Newtonsoft.Json.Linq.JObject;

using Catalog.Common.Service;
using Catalog.Common.Utils;

namespace Catalog.Common.Repository
{
	using KVParams = Dictionary<String, String>;

	#region Caches

	static class Cache
	{
		public static bool Verbose { get; set; }

		private static Dictionary<int, String> ImageName { set; get; } = new Dictionary<Int32, String>();

		private static Dictionary<int, StockLevel> StockLevel
		{ get; set; } = new Dictionary<int, StockLevel>();

		private static Dictionary<int, String> Pack { get; set; }
			= new Dictionary<int, string>();

		#region Methods

		public static void Add(int id, StockLevel stockLevel, String imageUri, String pack)
		{
			AddImageUri(id, imageUri);
			AddStock(id, stockLevel);
			AddPack(id, pack);
		}

		public static void AddImageUri(Int32 id, String uri)
		{
			try
			{
				ImageName.Add(id, uri.Trim());
			}
			catch
			{
				Debug.WriteLine($"Image for product with ID {id} does exist. Aborting");
			}
		}

		public static void AddStock(int id, StockLevel stockLevel)
		{
			try
			{
				StockLevel.Add(id, stockLevel);
			}
			catch
			{
				Debug.WriteLine($"StockLevel for product with ID {id} does exist. Aborting");
			}
		}

		public static void AddStock(int id, string stock1, string stock2)
		{
			Cache.AddStock(id, new StockLevel(stock1.Trim(), stock2.Trim()));
		}

		public static void AddPack(int id, string pack)
		{
			try
			{
				Pack.Add(id, pack.Trim());
			}
			catch
			{
				Debug.WriteLine($"Pack for product with ID {id} does exist. Aborting");
			}
		}

		public static bool TryGetImage(int id, out string value)
		{
			value = "";
			try
			{
				value = ImageName[id];
			}
			catch
			{
				if (Verbose)
					Debug.WriteLine($"Image for product with ID {id} doesn't exist in cache");
				return false;
			}
			return true;
		}

		public static bool TryGetStock(int id, out StockLevel stockLevel)
		{
			stockLevel = new StockLevel("-");
			try
			{
				stockLevel = StockLevel[id];
			}
			catch
			{
				if (Verbose)
					Debug.WriteLine($"Stock for product with ID {id} doesn't exist in cache");
				return false;
			}

			return true;
		}

		public static bool TryGetPack(int id, out string value)
		{
			value = "";
			try
			{
				value = Pack[id];
			}
			catch
			{
				if (Verbose)
					Debug.WriteLine($"Pack for product with ID {id} doesn't exist in cache");
				return false;
			}

			return true;
		}

		#endregion
	}

	#endregion

	#region WebRepository

	public partial class WebRepository
	{
		public static void GetProductCategories()
		{
			UpdateRequested = true;
			var response = FetchProductCategories().Result;

			if (response is null)
				return;

			var categoryNames = new List<string>();

			foreach (var pc in MainRepository.ProductCategoriesCache)
			{
				categoryNames.Add(pc.Name);
			}

			foreach (var categoryObj in response)
			{
				var categoryName = categoryObj["category"].ToObject<String>();
				if (categoryNames.Find(c => c.Equals(categoryName)) != null)
					continue;
				var category = new ProductCategory(categoryName);
				category.Save();
			}

			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.CATEGORY);
		}

		public static void GetProductSubcategories()
		{
			foreach (var category in MainRepository.ProductCategoriesCache)
			{
				//var subcategoryId = 1;
				var response = FetchSubcategories(category.Name).Result;

				if (response == null)
					continue;

				foreach (JsonObject data in response)
				{
					for (int index = 0; index < 2; ++index)
					{
						var key = $"subcategory{index}";
						if (!data.ContainsKey(key))
							continue;
						var subcategoryName = data[key].ToObject<String>();
						var exists = MainRepository.ProductSubcategoriesCache.Exists(
							s => Base64.Encode(s.Name) == Base64.Encode(subcategoryName)
						//|| s.ProductCategoryID == subcategoryId
						);

						if (!exists)
						{
							var productSubcategory =
								new ProductSubcategory(category.ProductCategoryID, subcategoryName);

							productSubcategory.Save();
						}
					}
				}
			}

			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.SUBCATEGORY);
		}

		public static void GetProducts()
		{
			var responses = FetchProducts().Result;
			MainRepository.ResetCache(CacheType.PRODUCT);

			foreach (var response in responses)
			{
				foreach (JsonObject data in response)
				{
					var product = new Product(
						data["id"].ToObject<Int32>(),
						data["artikul"].ToObject<String>(),
						data["kod"].ToObject<String>(),
						data["brend"].ToObject<String>(),
						data["name"].ToObject<String>(),
						data["description"].ToObject<String>(),
						Converter.ToDecimal(data["price"].ToObject<String>())
					);

					var id = product.ProductID;
					var stock1 = data["sklad1"].ToObject<String>();
					var stock2 = data["sklad2"].ToObject<String>();
					var imageUri = data["image"].ToObject<String>();
					var pack = data["upakovka"].ToObject<String>();

					Cache.Add(id, new StockLevel(stock1, stock2), imageUri, pack);

					var sub1 = data["subcategory1"].ToObject<String>();
					var sub2 = data.ContainsKey("subcategory2")
										? data["subcategory2"].ToObject<String>()
										: String.Empty;

					var productSubcategory = MainRepository.ProductSubcategoriesCache
												.Find(c => c.Name.ToLower().Equals(sub1.ToLower()));

					if (!sub2.Equals(String.Empty) && (productSubcategory == null))
					{
						var sub2Lower = sub2.ToLower();
						productSubcategory = MainRepository.ProductSubcategoriesCache
														   .Where(s => s.Name.ToLower().Equals(sub2Lower))
														   .FirstOrDefault();
					}

					if (productSubcategory != null)
						product.ProductSubcategoryID = productSubcategory.ProductSubcategoryID;

					var dbItem = Repository.Context
											.Products
											.Where(p => id == p.ProductID)
											.SingleOrDefault();
					if (dbItem != null)
					{
						ProductInventoryUpdateCount++;
						dbItem.Update(product);
					}
					else
						product.Save();
				}
			}

			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.PRODUCT);
		}

		public static void GetProductInventories()
		{
			var shoppingCart = GetShoppingCart();

			foreach (var product in MainRepository.ProductsCache)
			{
				Cache.TryGetStock(product.ProductID, out StockLevel stock);
				Cache.TryGetPack(product.ProductID, out string pack);

				var productInventory = new ProductInventory(product, stock, pack);

				var dbItem = Repository.Context
										.ProductInventories
										.Where(pi => product.ProductID == pi.ProductID)
										.SingleOrDefault();
				if (dbItem == null)
					productInventory.Save();
				else
					dbItem.Update(productInventory);

				GetShoppingCartItem(shoppingCart, product);
			}

			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.INVENTORY);
		}

		private static ShoppingCart GetShoppingCart()
		{
			ShoppingCart shoppingCart = null;
			shoppingCart = Repository.Context.ShoppingCarts
							.Include(sc => sc.ShoppingCartItems)
							.FirstOrDefault();

			if (shoppingCart == null)
			{
				shoppingCart = new ShoppingCart();
				shoppingCart.Save();
			}

			return shoppingCart;
		}

		private static void GetShoppingCartItem(ShoppingCart cart, Product product)
		{
			ShoppingCartItem cartItem = null;

			cartItem = Repository.Context.ShoppingCartItems
										 .Where(item => product.ProductID == item.ProductID)
										 .SingleOrDefault();

			if (cartItem is null)
			{
				cartItem = new ShoppingCartItem(cart, product);
				cartItem.Save();
			}
		}

		static void GetProductPhoto(Product product, String filename, Boolean saveImage = false)
		{
			void action(byte[] bytes)
			{
				using (var memstream = new MemoryStream(bytes))
				{
					var bitmap = System.Drawing.Image.FromStream(memstream);

					var photo = new ProductPhoto(filename, product, bitmap);
					var dbPhoto = Repository.Context
											.ProductPhotoes
											.Where(p => p.ProductID == product.ProductID)
											.SingleOrDefault();

					if (dbPhoto == null)
						photo.Save();
					else
						dbPhoto.Update(photo);
				}
			}

			using (var manager = new DownloadManager(Utils.Configuration.Get("imageURL")))
			{
				manager.DownloadAsBytes(filename, action);
			};
		}

		public static void GetProductPhotos()
		{
			if (!FetchImages)
				PhotosLoaded = true;

			if (!HasConnection)
				return;

			foreach (var product in MainRepository.ProductsCache)
			{
				Cache.TryGetImage(product.ProductID, out string filename);

				if (filename.Length == 0 || !FetchImages)
					continue;
				GetProductPhoto(product, filename);
			}
			EntityModel.Commit();
			MainRepository.ResetCache(CacheType.PHOTO);
			PhotosLoaded = true;
		}

		public async static void SaveOrder(int orderID, int productID, int productQuantity)
		{
			var user = Repository.Context.Users.SingleOrDefault();
			var userId = user != null ? user.UserID : 0;
			var saveOrderItem = new OrderSave(userId, orderID, productID, productQuantity);
			var serializerOptions = new Newtonsoft.Json.JsonSerializerSettings
			{
				StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii,
				FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Decimal,
				Culture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU")
			};
			var order = Newtonsoft.Json.JsonConvert.SerializeObject(saveOrderItem, serializerOptions);
			order = order.Replace("\"", "");

			var parameters = new KVParams
			{
				["type"] = "saveorder",
				["v"] = order
			};

			await MakePostRequest<String>(parameters, order, "");
		}

		public static void SendMail(
			byte[] file,
			string filename,
			string message = "Отправлено с ТД СБМ-Волга Каталог"
		)
		{
			string path = "sendOrder.php";

			Task.Run(async () =>
			{
				using (var content = new MultipartFormDataContent("NKdKd9Yk"))
				{
					content.Headers.ContentType.MediaType = "multipart/form-data";
					content.Add(
						new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(message))),
						"text"
					);
					content.Add(
						new StreamContent(new MemoryStream(file), file.Length),
						"myfile[]",
						filename
					);

					using (var client = new HttpClient())
					{
						client.DefaultRequestHeaders.Accept.Add(
							new MediaTypeWithQualityHeaderValue("multipart/form-data")
						);
						string responseMessage = "";
						try
						{
							var response = await client.PostAsync(RestAPIManager.ApiUrl + path, content);
							responseMessage = await response.Content.ReadAsStringAsync();
							HasConnection = true;
						}
						catch (Exception e)
						{
							Debug.Print($"Failed to post to " +
							$"{RestAPIManager.ApiUrl}{path}\n" +
							$"Reason: {e.Message}\n" +
							$"Response: {responseMessage}");
						}
					}
				}
			});
		}

		/// <summary>
		/// Request handler for products list
		/// </summary>
		/// 
		/// Endpoint: api/opt/v1/?type=categorie&v=base64encode(category.Name)
		/// <returns></returns>
		private async static Task<List<List<JsonObject>>> FetchProducts()
		{
			var responses = new List<List<JsonObject>>();
			foreach (var category in MainRepository.ProductCategoriesCache)
			{
				var parameters = new KVParams { ["type"] = "categorie", ["v"] = category.Name };
				var response = await MakeGetRequest<List<JsonObject>>(parameters);

				if (response != null)
					responses.Add(response);
			}
			return responses;
		}

		private async static Task<List<JsonObject>> FetchProductCategories()
		{
			var parameters = new KVParams { ["type"] = "allcategories" };
			return await MakeGetRequest<List<JsonObject>>(parameters);
		}

		private async static Task<List<JsonObject>> FetchSubcategories(String productCategoryName)
		{
			var parameters = new KVParams { ["type"] = "subcategorie", ["v"] = productCategoryName };
			return await MakeGetRequest<List<JsonObject>>(parameters);
		}

		private async static Task<TResult> MakeGetRequest<TResult>(KVParams paths)
		{
			var path = "?";
			var index = 0;
			foreach (var p in paths)
			{
				string key = p.Key;
				string val = p.Value;

				if (key == "v")
				{
					val = Base64.Encode(val);
				}
				if (val.Length > 0)
					path += $"{key}={val}";
				else
					path += $"{key}";

				if (index++ < paths.Count - 1)
					path += "&";
			}

			return await Task.Run(() =>
			{
				TResult result = default;
				try
				{
					result = Client.Get<TResult>(RestAPIManager.ApiUrl, path).GetResult();
					HasConnection = true;
				}
				catch (Exception e)
				{
					HasConnection = false;
					Debug.Print($"Failed to get from {RestAPIManager.ApiUrl}{path}\nReason: {e.Message}");
				}
				return result;
			});
		}

		private async static void MakePostRequest<TResult>(String path, object data)
		{
			await Task.Run(() =>
			{
				try
				{
					Client.Post<TResult>(RestAPIManager.ApiUrl, path, data);
					HasConnection = true;
				}
				catch (Exception e)
				{
					HasConnection = false;
					Debug.Print($"Failed to post to {RestAPIManager.ApiUrl}{path}\nReason: {e.Message}");
				}
				data = null;
			});
		}

		private async static Task<TResult> MakePostRequest<TResult>(
			KVParams paths,
			object data,
			string endpoint
		)
		{
			var path = "?";
			var index = 0;
			foreach (var p in paths)
			{
				string key = p.Key;
				string val = p.Value;

				if (val.Length > 0)
					path += $"{key}={val}";
				else
					path += $"{key}";

				if (index++ < paths.Count - 1)
					path += "&";
			}

			return await Task.Run(() =>
			{
				TResult result = default;
				string url = RestAPIManager.ApiUrl + endpoint;
				try
				{
					result = Client.Post<TResult>(url, path, data).GetResult();
					HasConnection = true;
				}
				catch (Exception e)
				{
					HasConnection = false;
					Debug.Print($"Failed to post to {url}{path}\nReason: {e.Message}");
				}
				return result;
			});
		}
	}

	#endregion
}
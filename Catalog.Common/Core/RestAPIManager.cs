using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using Catalog.RestClient;
using Catalog.Common.Repository;

using KVParams = System.Collections.Generic.Dictionary<System.String, System.String>;
using JsonObject = Newtonsoft.Json.Linq.JObject;

namespace Catalog.Common
{
	public static class RestAPIManager
	{
		public static Boolean HasConnection { get; set; } = true;

		public static JsonRestClient Client { get; private set; } = new JsonRestClient();

		#region Fetching data

		public async static Task<List<JsonObject>> FetchProductCategories()
		{
			var parameters = new KVParams
			{
				[API.CATEGORIES_TYPE_KEY] = API.CATEGORIES_PARAM
			};
			return await Get<List<JsonObject>>(parameters);
		}

		public async static Task<List<JsonObject>> FetchSubcategories(String productCategoryName)
		{
			var parameters = new KVParams
			{
				[API.SUBCATEGORIES_TYPE_KEY] = API.SUBCATEGORIES_PARAM,
				[API.SUBCATEGORIES_VALUE_KEY] = productCategoryName
			};
			return await Get<List<JsonObject>>(parameters);
		}

		/// <summary>
		/// Request handler for products list
		/// </summary>
		/// 
		/// Endpoint: api/opt/v1/?type=categorie&v=base64encode(category.Name)
		/// <returns></returns>
		public async static Task<List<List<JsonObject>>> FetchProducts()
		{
			var responses = new List<List<JsonObject>>();
			foreach (var category in MainRepository.ProductCategoriesCache)
			{
				var parameters = new KVParams
				{
					[API.PRODUCTS_TYPE_KEY] = API.PRODUCTS_PARAM,
					[API.PRODUCTS_VALUE_KEY] = category.Name
				};
				var response = await Get<List<JsonObject>>(parameters);

				if (response != null)
					responses.Add(response);
			}
			return responses;
		}

		public async static Task<List<JsonObject>> FetchProduct(String productCode)
		{
			var parameters = new KVParams
			{
				[API.PRODUCT_TYPE_KEY] = API.PRODUCT_PARAM,
				[API.PRODUCT_VALUE_KEY] = productCode
			};
			return await Get<List<JsonObject>>(parameters);
		}

		public async static Task<TResult> SaveOrder<TResult>(KVParams paths, Object data, String endpoint)
		{
			return await Post<TResult>(paths, data, endpoint);
		}

		#endregion

		#region REST API Helper methods

		/// <summary>
		/// Sends get request with path parameters
		/// </summary>
		/// <typeparam name="TResult">
		///		Returning value type
		/// </typeparam>
		/// <param name="paths">
		///		Method parameters
		/// </param>
		/// <returns>
		///		List of json data
		/// </returns>
		private async static Task<TResult> Get<TResult>(KVParams paths)
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
					result = Client.Get<TResult>(API.URL, path).GetResult();
					HasConnection = true;
				}
				catch (Exception e)
				{
					HasConnection = false;
					Debug.Print($"Failed to get from {API.URL}{path}\nReason: {e.Message}");
				}
				return result;
			});
		}

		private async static Task<TResult> Post<TResult>(
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
				string url = API.URL + endpoint;
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

		private async static void Post<TResult>(String path, Object data)
		{
			await Task.Run(() =>
			{
				try
				{
					Client.Post<TResult>(API.URL, path, data);
					HasConnection = true;
				}
				catch (Exception e)
				{
					HasConnection = false;
					Debug.Print($"Failed to post to {API.URL}{path}\nReason: {e.Message}");
				}
				data = null;
			});
		}

		#endregion
	}
}

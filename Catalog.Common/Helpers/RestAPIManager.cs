using System;
using System.Configuration;
using Catalog.RestClient;

namespace Catalog.Common
{
	public static class RestAPIManager
	{
		public static Boolean HasConnection { get; set; } = true;

		public static JsonRestClient Client
		{
			get; private set;
		}

		public static string ApiUrl
		{
			get { return "https://ivanshop.ru/api/opt/v1/"; }
		}

		static RestAPIManager()
		{
			Client = new JsonRestClient();
		}
	}
}

using System;
using System.Diagnostics;

using Catalog.RestClient;

namespace Catalog.Common.Repository
{
	public partial class WebRepository
	{
		public static JsonRestClient Client => RestAPIManager.Client;

		public static int ProductInventoryUpdateCount = 0;

		public static bool HasConnection
		{
			get => RestAPIManager.HasConnection;
			set => RestAPIManager.HasConnection = value;
		}

		public static bool UpdateRequested {get; set;}

		public static bool FetchImages { get; set; } = true;

		public static bool PhotosLoaded { get; private set; } = false;
	}
}
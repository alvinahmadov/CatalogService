namespace Catalog.Common.Repository
{
	public partial class WebWorker
	{
		public static System.Action<System.String, System.Object> LoggingCallback { get; set; }

		public static System.Action SuccessCallback { get; set; }

		public static System.Boolean HasConnection
		{
			get => RestAPIManager.HasConnection;
			set => RestAPIManager.HasConnection = value;
		}

		public static UpdateStatus UpdateStatus { get; set; } = UpdateStatus.Default;

		public static System.Boolean PhotosUpdating { get; private set; } = false;
	}
}
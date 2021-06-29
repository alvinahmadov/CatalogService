using System;
using System.Linq;

using Catalog.Common.Repository;
using Catalog.Common.Service;

using DatabaseSettings = Catalog.Common.Service.Settings;

namespace Catalog.Client.Properties
{
	internal sealed partial class Settings
	{
		public Settings()
		{
			if (dbSettings is null)
			{
				dbSettings = new DatabaseSettings
				{
					ID = 1,
					ModifiedDate = DateTime.Now,
					UpdateInterval = 30,
				};

				dbSettings.Save(true, true);
			} 
		}

		public static bool AskConfirmation
		{
			get => dbSettings.AskConfirmation;
			set
			{
				if (dbSettings.AskConfirmation != value)
					dbSettings.AskConfirmation = value;
			}
		}

		public static bool LoadImages
		{
			get => dbSettings.LoadImage;
			set
			{
				if (dbSettings.LoadImage != value)
					dbSettings.LoadImage = value;
			}
		}

		public static int UpdateInterval
		{
			get => dbSettings.UpdateInterval;
			set
			{
				if (dbSettings.UpdateInterval != value)
					dbSettings.UpdateInterval = value;
			}
		}

		public static void Commit() 
		{
			dbSettings.ModifiedDate = DateTime.Now;
			EntityModel.Commit();
		}

		private static DatabaseSettings dbSettings = Repository.Context
																.Settings
																.SingleOrDefault();
	}
}

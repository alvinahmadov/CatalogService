using System;
using System.Linq;

using Catalog.Common.Repository;
using Catalog.Common.Service;

using DatabaseSettings = Catalog.Common.Service.Settings;

namespace Catalog.Client.Properties
{
	internal sealed partial class Settings
	{
		public bool AskConfirmation
		{
			get => dbSettings.AskConfirmation;
			set
			{
				if (dbSettings.AskConfirmation != value)
					dbSettings.AskConfirmation = value;
			}
		}

		public bool LoadImage
		{
			get => dbSettings.LoadImage;
			set
			{
				if (dbSettings.LoadImage != value)
					dbSettings.LoadImage = value;
			}
		}

		public int UpdateInterval
		{
			get => dbSettings.UpdateInterval;
			set
			{
				if (dbSettings.UpdateInterval != value)
					dbSettings.UpdateInterval = value;
			}
		}

		public void Commit()
		{
			dbSettings.ModifiedDate = DateTime.Now;
			EntityModel.Commit();
		}

		private DatabaseSettings dbSettings = Repository.Context
																.Settings
																.SingleOrDefault();
	}
}
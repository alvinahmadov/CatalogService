#pragma warning disable IDE0044 // Add readonly modifier

using System;
using System.Linq;

using Catalog.Common.Repository;
using Catalog.Common.Service;

using DatabaseSettings = Catalog.Common.Service.Settings;

namespace Catalog.Client.Properties
{
	internal sealed partial class Settings
	{
		public Int32 UpdateInterval
		{
			get => Database.UpdateInterval;
			set
			{
				if (Database.UpdateInterval != value)
					Database.UpdateInterval = value;
			}
		}

		public Int16 LeftPanelWidth
		{
			get => Database.LeftPanelWidth;
			set
			{
				var min = Common.GUI.COLLAPSIBLE_PANEL_MIN_WIDTH;
				var max = Common.GUI.COLLAPSIBLE_PANEL_MAX_WIDTH;

				if (dbSettings.LeftPanelWidth != value)
					if (value >= min && value <= max)
					{
						Database.LeftPanelWidth = value;
					}
			}
		}

		public Boolean AskConfirmation
		{
			get => Database.AskConfirmation;
			set
			{
				if (Database.AskConfirmation != value)
					Database.AskConfirmation = value;
			}
		}

		public Boolean LoadImage
		{
			get => Database.LoadImage;
			set
			{
				if (Database.LoadImage != value)
					Database.LoadImage = value;
			}
		}

		public void Commit()
		{
			dbSettings.ModifiedDate = DateTime.Now;
			Entity.Commit();
		}

		public DatabaseSettings Database
		{
			get
			{
				if (dbSettings == null)
				{
					dbSettings = new Common.Service.Settings(1);
					dbSettings.Save(true);
				}

				return dbSettings;
			}
		}

		private DatabaseSettings dbSettings = Repository.Context.Settings.SingleOrDefault();
	}
}
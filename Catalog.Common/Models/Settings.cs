using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.Service
{
	public partial class Settings : EntityModel
	{
		public Settings(int id)
		{
			this.ID = id;
			this.UpdateInterval = 30;
			this.AskConfirmation = true;
			this.LoadImage = true;
			this.ModifiedDate = DateTime.Now;
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
					Repository.Repository.Context.Settings.Add(this);

				if (commit)
				{
					Debug.WriteLine("Commit at Settings.Save");
					var entry = Repository.Repository.Context.Entry(this);
					Debug.WriteLine($"Settings entry => State: {entry.State}");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(object entity, bool commit = false)
		{
			var other = entity as Settings;

			if (this.AskConfirmation != other.AskConfirmation &&
				this.LoadImage != other.LoadImage &&
				this.UpdateInterval != other.UpdateInterval)
			{
				this.AskConfirmation = other.AskConfirmation;
				this.LoadImage = other.LoadImage;
				this.UpdateInterval = other.UpdateInterval;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.Settings.Remove(this);
			Save(false, true);
		}

	}
}

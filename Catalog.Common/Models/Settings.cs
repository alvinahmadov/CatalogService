using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.Service
{
	partial class Settings : EntityModel
	{
		
		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
					Repository.Repository.Context.Settings.Add(this);

				if (commit)
					Commit();
			}
			catch { }
		}

		public override void Update(object entity, bool commit = false)
		{
			var other = entity as Settings;

			if (this.AskConfirmation != other.AskConfirmation &&
				this.LoadImage != other.LoadImage &&
				this.UpdateInterval != other.UpdateInterval)
			{
				this.AskConfirmation = other.AskConfirmation;
				this.LoadImage = other.LoadImage;
				this.UpdateInterval = other.UpdateInterval;

				if (commit)
					Save(!commit, commit);
			}

		}

		public override void Delete()
		{
			Repository.Repository.Context.Settings.Remove(this);
			Commit();
		}

	}
}

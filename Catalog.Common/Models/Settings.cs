using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.Service
{
	public partial class Settings : Entity
	{
		public Settings(Int32 id)
		{
			this.ID = id;
			this.UpdateInterval = 30;
			this.AskConfirmation = true;
			this.LoadImage = true;
			this.LeftPanelWidth = GUI.DEFAULT_COLLAPSIBLE_PANEL_WIDTH;
			this.ModifiedDate = DateTime.Now;
		}

		public override void Save(Boolean commit = false)
		{
			Repository.Repository.Context.Settings.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
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
			Commit();
		}

		public override Boolean Equals(Object obj)
		{
			var entity = obj as Settings;

			var equals = (this.UpdateInterval == entity.UpdateInterval) &&
						 (this.AskConfirmation == entity.AskConfirmation) &&
						 (this.LoadImage == entity.LoadImage);

			return equals;
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override String ToString()
		{
			return $"Settings <[" +
			$"\tUpdateInterval: {this.UpdateInterval}, " +
			$"\tAskConfirmation: {this.AskConfirmation}, " +
			$"\tLoadImage: {this.LoadImage}, " +
			$"\tModifiedDate: {this.ModifiedDate}" +
			$"]>";
		}

	}
}

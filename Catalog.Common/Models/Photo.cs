using System;
using System.Collections.Generic;

namespace Catalog.Common.Service
{
	using PhotosList = List<Photo>;

	public partial class Photo : Entity
	{
		public Photo(Int32 id, String fileName)
		{
			this.ID = id;
			this.FileName = fileName;
			this.ThumbNailPhoto = new byte[0];
			this.LargePhoto = new byte[0];
			this.ModifiedDate = DateTime.Now;
		}

		public static void SaveRange(PhotosList range, bool commit = true)
		{
			Repository.Repository.Context.Photos.AddRange(range);

			if (commit)
				Commit();
		}

		public override void Save(bool commit = false)
		{
			Repository.Repository.Context.Photos.Add(this);

			if (commit)
				Commit();
		}

		public override Boolean Update(in Object entity)
		{
			if (entity is null)
				return false;

			var other = entity as Photo;

			if (!Equals(other))
			{
				this.ProductID = other.ProductID;
				this.FileName = other.FileName;
				this.ThumbNailPhoto = other.ThumbNailPhoto;
				this.LargePhoto = other.LargePhoto;
				this.Product = other.Product;
				this.ModifiedDate = DateTime.Now;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.Photos.Remove(this);
			Commit();
		}

		public override Int32 GetHashCode()
		{
			return this.ID.GetHashCode()
			^ this.ProductID.GetHashCode()
			^ this.FileName.GetHashCode();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as Photo;

			return (this.ProductID == other.ProductID)
				&& (this.Product.ProductID == other.Product.ProductID)
				&& (this.FileName == other.FileName)
				&& (this.LargePhoto == other.LargePhoto)
				&& (this.ThumbNailPhoto == other.ThumbNailPhoto);
		}

		public override String ToString()
		{
			return $"ProductPhoto ({this.ID}) <[" +
			$"ProductID: {this.ProductID}, " +
			$"FileName: {this.FileName}, " +
			$"Code: {this.Product?.Code}" +
			$"Has Image: {this.ThumbNailPhoto.Length > 0}" +
			$"]>";
		}
	}
}
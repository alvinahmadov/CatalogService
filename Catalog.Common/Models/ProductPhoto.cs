using System;
using System.Diagnostics;

namespace Catalog.Common.Service
{
	public partial class ProductPhoto : EntityModel
	{
		public ProductPhoto()
		{
			this.ThumbNailPhoto = new byte[0];
			this.LargePhoto = new byte[0];
			this.ModifiedDate = DateTime.Now;
		}

		public ProductPhoto(Int32 id, String fileName)
		{
			this.ID = id;
			this.FileName = fileName;
			this.ThumbNailPhoto = new byte[0];
			this.LargePhoto = new byte[0];
			this.ModifiedDate = DateTime.Now;
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
				{
					Repository.Repository.Context.ProductPhotos.Add(this);
				}

				if (commit)
				{
					Debug.WriteLine("Commit at ProductPhoto.Save");
					Commit();
				}
			}
			catch { }
		}

		public override bool Update(Object entity, bool commit = false)
		{
			var productPhoto = entity as ProductPhoto;

			if (this != productPhoto)
			{
				this.ProductID = productPhoto.ProductID;
				this.FileName = productPhoto.FileName;
				this.ThumbNailPhoto = productPhoto.ThumbNailPhoto;
				this.LargePhoto = productPhoto.LargePhoto;
				this.ModifiedDate = DateTime.Now;
				this.Product = productPhoto.Product;

				return true;
			}

			return false;
		}

		public override void Delete()
		{
			Repository.Repository.Context.ProductPhotos.Remove(this);
			Save(false);
		}

		public override String ToString()
		{
			return $"ProductPhoto<[" +
			$"ProductID: {this.ProductID}, " +
			$"FileName: {this.FileName}, " +
			$"Code: {this.Product?.Code}" + 
			$"Has Image: {this.ThumbNailPhoto.Length > 0}" +
			$"]>";
		}

		public override Int32 GetHashCode()
		{
			return base.GetHashCode();
		}

		public override Boolean Equals(Object obj)
		{
			if (obj == null)
				return false;

			var other = obj as ProductPhoto;

			return (this.ProductID == other.ProductID)
			&& (this.Product.ProductID == other.Product.ProductID)
			&& (this.FileName == other.FileName)
			&& (this.LargePhoto == other.LargePhoto)
			&& (this.ThumbNailPhoto == other.ThumbNailPhoto);
		}
	}
}
using System;
using System.Drawing;

using Catalog.Common.Utils;

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

		public ProductPhoto(string filename, Product product, Image image) : this()
		{
			this.ID = product.ID;
			this.ProductID = product.ProductID;
			this.FileName = filename;
			this.Product = product;

			try
			{
				var thumbnailPhoto = ImageUtils.GetThumbnailPhoto(ref image);
				var largePhoto = ImageUtils.GetLargePhoto(ref image);

				this.ThumbNailPhoto = ImageUtils.GetThumbnailPhotoBytes(ref thumbnailPhoto);
				this.LargePhoto = ImageUtils.GetLargePhotoBytes(ref largePhoto);
			}
			catch { }
		}

		public override void Save(bool isAddingItem = true, bool commit = false)
		{
			try
			{
				if (isAddingItem)
					Repository.Repository.Context.ProductPhotoes.Add(this);
				if (commit)
					Commit();
			}
			catch { }
		}

		public override void Update(Object entity, bool commit = false)
		{
			var productPhoto = entity as ProductPhoto;
			if (
				this.FileName != productPhoto.FileName &&
				this.ThumbNailPhoto != productPhoto.ThumbNailPhoto &&
				this.LargePhoto != productPhoto.LargePhoto &&
				this.ProductID != productPhoto.ProductID
			)
			{
				this.ID = productPhoto.ID;
				this.ProductID = productPhoto.ProductID;
				this.FileName = productPhoto.FileName;
				this.ThumbNailPhoto = productPhoto.ThumbNailPhoto;
				this.LargePhoto = productPhoto.LargePhoto;
				this.ModifiedDate = DateTime.Now;
				this.Product = productPhoto.Product;

				if (commit)
					Save(!commit, commit);
			}
		}

		public override void Delete()
		{
			Repository.Repository.Context.ProductPhotoes.Remove(this);
			Save(false);
		}
	}
}
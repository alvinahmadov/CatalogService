using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Catalog.Common.Utils
{
	public class ImageUtils
	{
		private static int thumbnailSize;
		private static int defaultSize;

		public static int ThumbnailSize
		{
			get
			{
				if (thumbnailSize == 0) thumbnailSize = 60;
				return thumbnailSize;
			}
			set { if (value >= 0) thumbnailSize = value; }
		}

		public static int DefaultSize
		{
			get 
			{
				if (defaultSize == 0) defaultSize = 450;
				return defaultSize;
			}

			set { if (value >= 0) defaultSize = value; }
		}

		public static Image Resize(ref System.Drawing.Image image)
		{
			var destRect = new Rectangle(0, 0, ThumbnailSize, ThumbnailSize);
			var destImage = new Bitmap(ThumbnailSize, ThumbnailSize);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		public static Image Resize(ref System.Drawing.Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		public static byte[] ToByteArray(ref Image imageIn)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				imageIn.Save(ms, ImageFormat.Jpeg);
				return ms.ToArray();
			}
		}

		public static Image GetImage(byte[] bytes, Image defaultImage)
		{
			if (bytes == null || bytes.Length == 0)
				return defaultImage;

			using (var memstream = new System.IO.MemoryStream(bytes))
			{
				return Image.FromStream(memstream);
			}
		}

		public static Image GetImage(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;
			
			using (var memstream = new System.IO.MemoryStream(bytes))
			{
				return Image.FromStream(memstream);
			}
		}

		public static Image GetThumbnailPhoto(ref Image image)
		{
			return GetThumbNailImage<Image>(ref image) as Image;
		}

		public static Image GetLargePhoto(ref Image image)
		{
			return GetLargeImage<Image>(ref image) as Image;
		}

		public static Byte[] GetThumbnailPhotoBytes(ref Image image)
		{
			return GetThumbNailImage<Byte[]>(ref image) as Byte[];
		}

		public static Byte[] GetLargePhotoBytes(ref Image image)
		{
			return GetLargeImage<Byte[]>(ref image) as Byte[];
		}

		private static Object GetThumbNailImage<T>(ref Image image)
		{
			var thumbnail = image.GetThumbnailImage(ThumbnailSize, ThumbnailSize, () => false, new IntPtr(0));

			if (typeof(T) == typeof(Image))
			{
				return thumbnail;
			}
			else if (typeof(T) == typeof(Byte[]))
			{
				var byteArray = ToByteArray(ref thumbnail);
				return byteArray;
			}
			return default;
		}

		private static Object GetLargeImage<T>(ref Image image)
		{
			var largePhoto = Resize(ref image, DefaultSize, DefaultSize);
			if (typeof(T) == typeof(Image))
			{
				return largePhoto;
			}
			else if (typeof(T) == typeof(Byte[]))
			{
				var byteArray = ToByteArray(ref largePhoto);
				return byteArray;
			}
			return default;
		}
	}
}

using System;
using System.Linq;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

using JsonToken = Newtonsoft.Json.Linq.JToken;

namespace Catalog.Common
{
	public delegate void EventAction<TEvent>(Object sender, TEvent eventArgs);

	public class GridValueChangedEventArgs : EventArgs
	{
		public int DataId { get; set; }

		public int OldValue { get; set; }

		public int NewValue { get; set; }

		public GridValueChangedEventArgs(int dataId, int oldValue, int newValue)
		{
			this.DataId = dataId;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}
	}

	public struct OrderSave
	{
		public int userid;
		public int ordernumber;
		public int product;
		public int quantity;

		public OrderSave(int userId, int orderNumber, int productId, int productQuantity)
		{
			userid = userId;
			ordernumber = orderNumber;
			product = productId;
			quantity = productQuantity;
		}
	}

	public struct Base64
	{
		public enum OpMode
		{
			ENCODE,
			DECODE
		}

		/// <summary>
		/// Decodes givent string to base64
		/// </summary>
		/// <param name="plainText">Text to be encoded</param>
		/// <returns>Encoded text</returns>
		public static string Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		/// <summary>
		/// Decodes given token
		/// </summary>
		/// <param name="base64EncodedData">Data to be decoded</param>
		/// <returns>Decoded string</returns>
		public static string Decode(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static bool Equal(string text1, string text2) => Equal(text1, text2, OpMode.ENCODE);

		public static bool Equal(string text1, string text2, OpMode op)
		{
			if (op.Equals(OpMode.ENCODE))
				return Encode(text1) == Encode(text2);
			else
				return Decode(text1) == Decode(text2);
		}
	}

	public struct Converter
	{
		public static Decimal ToDecimal(string token)
		{
			token = token.Trim();
			if (token.Length == 0)
				return 0.00M;
			if (token.EndsWith(",") || token.EndsWith("."))
				token = token.Substring(0, token.Length - 1);

			if (Regex.Match(token, @"\s", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Success)
				token = Regex.Replace(token, @"\s", "");


			Decimal.TryParse(
				token,
				NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol,
				CultureInfo.InvariantCulture,
				out Decimal value
			);

			return value;
		}

		public static Double ToDouble(JsonToken token)
		{
			return ToFloatingPoint<Double>(token);
		}

		private static dynamic ToFloatingPoint<T>(JsonToken token)
		{
			var str = token.ToObject<string>();

			str = str.Replace('.', ',');
			str = Regex.Replace(str, @"\s", string.Empty);

			if (str.Length == 0)
				str = "0,0";
			else if (!str.Contains(","))
				str = $"{str},0";


			if (typeof(T) == typeof(Decimal))
			{
				return Convert.ToDecimal(str);
			}
			else if (typeof(T) == typeof(Double))
			{
				return Convert.ToDouble(str);
			}

			return default;
		}
	}

	public static class ImageUtil
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

		public static System.Drawing.Image Resize(ref System.Drawing.Image image)
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

		public static System.Drawing.Image Resize(ref System.Drawing.Image image, int width, int height)
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

		public static byte[] ToByteArray(ref System.Drawing.Image imageIn)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				imageIn.Save(ms, ImageFormat.Jpeg);
				return ms.ToArray();
			}
		}

		public static System.Drawing.Image GetImage(byte[] bytes, System.Drawing.Image defaultImage)
		{
			if (bytes == null || bytes.Length == 0)
				return defaultImage;

			using (var memstream = new System.IO.MemoryStream(bytes))
			{
				return System.Drawing.Image.FromStream(memstream);
			}
		}

		public static System.Drawing.Image GetImage(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return null;

			using (var memstream = new System.IO.MemoryStream(bytes))
			{
				return System.Drawing.Image.FromStream(memstream);
			}
		}

		public static System.Drawing.Image GetThumbnailPhoto(ref System.Drawing.Image image)
		{
			return GetThumbNailImage<System.Drawing.Image>(ref image) as System.Drawing.Image;
		}

		public static System.Drawing.Image GetLargePhoto(ref System.Drawing.Image image)
		{
			return GetLargeImage<System.Drawing.Image>(ref image) as System.Drawing.Image;
		}

		public static Byte[] GetThumbnailPhotoBytes(ref System.Drawing.Image image)
		{
			return GetThumbNailImage<Byte[]>(ref image) as Byte[];
		}

		public static Byte[] GetLargePhotoBytes(ref System.Drawing.Image image)
		{
			return GetLargeImage<Byte[]>(ref image) as Byte[];
		}

		private static Object GetThumbNailImage<T>(ref System.Drawing.Image image)
		{
			var thumbnail = image.GetThumbnailImage(ThumbnailSize, ThumbnailSize, () => false, new IntPtr(0));

			if (typeof(T) == typeof(System.Drawing.Image))
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

		private static Object GetLargeImage<T>(ref System.Drawing.Image image)
		{
			var largePhoto = Resize(ref image, DefaultSize, DefaultSize);
			if (typeof(T) == typeof(System.Drawing.Image))
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

	public static class SubcategoryHelper
	{
		public static void Set(
			ProductJSON json,
			in Service.Product product,
			out String subcategory1, 
			out String subcategory2
		)
		{
			subcategory1 = json.Sub1;
			subcategory2 = json.Data.ContainsKey("subcategory2")
								? json.Sub2
								: String.Empty;
			var subNameLower = subcategory1.ToLower();
			var dbSubcategory = Repository.MainRepository.SubcategoriesCache
											  .Find(c => c.Name.ToLower().Equals(subNameLower));

			var productSubcategory = dbSubcategory;

			if (!subcategory2.Equals(String.Empty))
			{
				if (dbSubcategory != null)
				{
					productSubcategory = Repository.MainRepository.SubcategoriesCache
										.Where(s => s.ChildSubcategoryID == dbSubcategory.ChildSubcategoryID)
										.FirstOrDefault();
				}
				else
				{
					var sub2Lower = subcategory2.ToLower();
					dbSubcategory = Repository.MainRepository.SubcategoriesCache
												  .Where(s => s.Name.ToLower().Equals(sub2Lower))
												  .FirstOrDefault();
				}

				productSubcategory = dbSubcategory;
			}
			else
				productSubcategory = dbSubcategory;

			var subId = productSubcategory.SubcategoryID;

			product.SubcategoryID = subId;
			product.Subcategory = Repository.MainRepository.SubcategoriesCache.Find(c => c.SubcategoryID == subId);
		}
	}
}

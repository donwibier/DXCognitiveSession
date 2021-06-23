using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace DXVision.BlazorApp.Server.Utils
{
    public static class ImageUtils
    {

        /// <summary>
        /// Scales an image proportionally.  Returns a bitmap.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            Bitmap bmp = new Bitmap(newImage);

            return bmp;
        }

        public static Bitmap ScaleImage(Stream image, int maxWidth, int maxHeight)
        {
			Image img = Image.FromStream(image);
            return ScaleImage(img, maxWidth, maxHeight);
        }

        public static void WritePNG(Bitmap bmp, Stream output)
        {
            bmp.Save(output, ImageFormat.Png);
        }

        public static Image CreateImage(Stream data)
		{
            Image result = Image.FromStream(data);
            return result;
        }

		public static Image CreateImage(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
                var result = CreateImage(stream);
                return result;
			}
		}

		public static byte[] GetBytes(Image image, ImageFormat format)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				image.Save(stream, format);
				return stream.ToArray();
			}
		}

        public static Stream GetStream(Image image, ImageFormat format)
		{
            MemoryStream stream = new MemoryStream();
            image.Save(stream, format);
            return stream;
        }
    }

    //Stream img = new MemoryStream();
    //ImageProcessingUtility.WritePNG(ImageProcessingUtility.ScaleImage(imageContent, 1200, 800), img);

}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace DXVisionSample.Code
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
        static public Bitmap ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
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

        static public Bitmap ScaleImage(Stream image, int maxWidth, int maxHeight)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(image);
            return ScaleImage(img, maxWidth, maxHeight);
        }

        static public void WritePNG(Bitmap bmp, Stream output)
        {
            bmp.Save(output, System.Drawing.Imaging.ImageFormat.Png);
        }

		static public Image CreateImage(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				Image result = Image.FromStream(stream);
				return result;
			}
		}

		static public byte[] GetBytes(Image image, ImageFormat format)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				image.Save(stream, format);
				return stream.ToArray();
			}
		}
    }

    //Stream img = new MemoryStream();
    //ImageProcessingUtility.WritePNG(ImageProcessingUtility.ScaleImage(imageContent, 1200, 800), img);

}
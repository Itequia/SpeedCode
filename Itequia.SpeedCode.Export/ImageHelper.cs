using System.Drawing;
using System.Drawing.Drawing2D;

namespace Itequia.SpeedCode.Export
{
    public static class ImageHelper
    {
        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);                
                return image;
            }
        }
        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static MemoryStream Base64ToStream(string base64String)
        {            
            byte[] imageBytes = Convert.FromBase64String(base64String);            
            return new MemoryStream(imageBytes, 0, imageBytes.Length);
            
        }

        public static byte[] ResizeImage(byte[] buffer, int width, int height)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                Bitmap thumb = new Bitmap(width, height);
                using (Image bmp = Image.FromStream(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(bmp, 0, 0, width, height);
                    }
                }
                // a picturebox to show/test the result
                return ImageToByte2(thumb);
            }
        }

        public static byte[] ResizeImage(string base64String, int width, int height)
        {
            base64String = base64String.Replace("data:image/jpeg;base64,", "");
            byte[] buffer = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                Bitmap thumb = new Bitmap(width, height);
                using (Image bmp = Image.FromStream(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.DrawImage(bmp, 0, 0, width, height);
                    }
                }
                // a picturebox to show/test the result
                return ImageToByte2(thumb);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Seasar.Fisshplate.Util
{
    public static class ImageIOUtil
    {
        private static ImageConverter _imgConv = new ImageConverter();

        public static Image Read(FileStream fs)
        {
            return Image.FromStream(fs, false, false);
        }
        public static void Write(Image img, string suffix, Stream s )
        {
            ImageFormat format = null;
            switch (suffix)
            {
                case "jpg":
                    format = ImageFormat.Jpeg;
                    break;
                case "png":
                    format = ImageFormat.Png;
                    break;
                default :
                    format = ImageFormat.MemoryBmp;
                    break;
            }

            img.Save(s, format);
        }

        public static void Close(Stream s)
        {
            if (s != null)
            {
                s.Close();
            }
        }

        public static byte[] ConvertToBytes(Image img)
        {
            return (byte[])_imgConv.ConvertTo(img, typeof(byte[]));
        }
    }
}

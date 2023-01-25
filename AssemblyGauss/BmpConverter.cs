using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using PixelFormat = System.Windows.Media.PixelFormat;
using System.Windows.Interop;

namespace AssemblyGauss
{
    public static class BmpConverter
    {
        public static BitmapSource BmpBGRArrayToImage(
           this byte[] pixels, int width, int height, PixelFormat pixelFormat)
        {
            const int bitsInByte = 8;
            const int dpi = 96;
            WriteableBitmap bitmap = new WriteableBitmap(width, height, dpi, dpi, pixelFormat, null);

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * (bitmap.Format.BitsPerPixel / bitsInByte), 0);

            return bitmap;
        }

        public static float[] ToFloatArray(this byte[] byteArray)
        {
            var newFloatArray = new float[byteArray.Length];
            for (int i = 0; i < newFloatArray.Length; i++)
            {
                newFloatArray[i] = (float)byteArray[i];
            }
            return newFloatArray;
        }
        public static float[] ToBmpBGRArray(this BitmapSource bitmapSource)
        {
            int stride = bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] bytePixels = new byte[bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(bytePixels, stride, 0);

            float[] floatPixels = bytePixels.ToFloatArray();

            return floatPixels;
        }

        public static Bitmap BitmapFromSource(BitmapSource source)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap source)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                source.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public static BitmapSource BmpBGRArrayToImage(
                this float[] pixels, int width, int height, PixelFormat pixelFormat)
        {
            byte[] byteArray = pixels.ToByteArray();
            return byteArray.BmpBGRArrayToImage(width, height, pixelFormat);
        }

        public static byte[] ToByteArray(this float[] floatArray)
        {
            var newByteArray = new byte[floatArray.Length];
            for (int i = 0; i < newByteArray.Length; i++)
            {
                newByteArray[i] = (byte)floatArray[i];
            }
            return newByteArray;
        }
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Stroke_detection
{
    public static class BitmapHelper
    {
        public static BitmapSource ToBitmapImage(ushort[] imageData, int width, int height, int winMin,
            int winMax)
        {
            byte[] lut16 = new byte[65536];

            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / range;

            for (int i = 0; i < 65536; ++i)
            {
                if (i <= winMin)
                    lut16[i] = 0;
                else if (i >= winMax)
                    lut16[i] = 255;
                else
                {
                    lut16[i] = (byte)((i - winMin) * factor);
                }
            }

            List<byte> list = new List<byte>();

            foreach (var pixel in imageData)
            {
                list.Add(lut16[pixel]);
            }

            var buffer8Bit = list.ToArray();

            return BitmapSource.Create(width, height,
                300, 300, PixelFormats.Gray8, BitmapPalettes.Gray256,
                buffer8Bit, width);
        }

        /// <summary>
        /// Gets luminosity values from BitmapSource.
        /// </summary>
        /// <param name="bitmap">BitmapSource to be processed.</param>
        /// <returns> int[,] array of values of bitmap</returns>
        public static int[,] GetValuesFromBitmapSource(BitmapSource bitmap)
        {
            int height = bitmap.PixelHeight;
            int width = bitmap.PixelWidth;

            int[,] values = new int[height,width];

            int stride = width * (bitmap.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[height * stride];

            bitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    values[i, j] = pixels[i * stride + j];
                }
            }

            return values;
        }

        /// <summary>
        /// Gets BitmapSource from values.
        /// </summary>
        /// <param name="values">values of luminosity in in[,] array</param>
        /// <returns>BitmapSource with set values</returns>
        public static BitmapSource GetBitmapSourceFromValues(int[,] values)
        {
            //TODO: wkleić wartości int w bitmapSource
            throw new NotImplementedException();
        }
    }
}
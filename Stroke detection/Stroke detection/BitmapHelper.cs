using System;
using System.Collections.Generic;
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
    }
}
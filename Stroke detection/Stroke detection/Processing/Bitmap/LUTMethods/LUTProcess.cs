using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.LUTMethods
{
    /// <summary>
    /// Class for Look Up Table Processing.
    /// Implements IProcBitmap.
    /// </summary>
    public class LUTProcess : IProcBitmap
    {
        /// <summary>
        /// Look Up Table. Starts from 0;
        /// </summary>
        private int[] LUT;

        /// <summary>
        /// Sets new LUT
        /// </summary>
        /// <param name="LUT">int[] LUT to be set</param>
        public void SetLUT(int[] LUT)
        {
            this.LUT = LUT;
        }

        /// <summary>
        /// Processes bitmap using LUT.
        /// </summary>
        /// <param name="bitmap"> BitmapSource to be processed.</param>
        /// <returns> BitmapSource of processed bitmap. Null if bitmap or LUT were null.</returns>
        public BitmapSource Process(BitmapSource bitmap)
        {
            BitmapSource outcome = null;
            if (LUT != null && bitmap != null)
            {
                int[,] values = BitmapHelper.GetValuesFromBitmapSource(bitmap);
                values = ProcessInt(values);
                outcome = BitmapHelper.GetBitmapSourceFromValues(values);
            }
            return outcome;
        }

        /// <summary>
        /// Processes bitmap using LUT. Operates on int.
        /// </summary>
        /// <param name="values">int[,] of values to be processed.</param>
        /// <returns>int[,] of processed values. Null if bitmap or LUT were null. </returns>
        public int[,] ProcessInt(int[,] values)
        {
            int[,] outcome = null;
            if (LUT != null && values != null)
            {
                outcome = new int[values.GetLength(0), values.GetLength(1)];
                Parallel.For(0, values.GetLength(0), x =>
                {
                    for (int y = 0; y < values.GetLength(1); y++)
                    {
                        outcome[x, y] = LUT[ClampToLutRange(values[x, y])];
                    }
                });
            }
            return outcome;
        }

        /// <summary>
        /// Clamps value to range of LUT;
        /// </summary>
        /// <param name="val"> int value to be clamped</param>
        private int ClampToLutRange(int val)
        {
            return Math.Max(0, Math.Min(LUT.Length - 1, val));
        }
    }
}

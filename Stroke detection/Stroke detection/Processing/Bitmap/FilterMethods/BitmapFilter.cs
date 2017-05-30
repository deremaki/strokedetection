using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.FilterMethods
{
    /// <summary>
    /// Class for matrix filters on BitmapSource.
    /// Implements IProcBitmap
    /// </summary>
    public class BitmapFilter : IProcBitmap
    {
        /// <summary>
        /// Mask of the filter.
        /// </summary>
        private int[,] mask;

        /// <summary>
        /// Radius of mask;
        /// </summary>
        private int maskRadius;

        /// <summary>
        /// Weight of mask (summed values).
        /// </summary>
        private int maskWeight;

        /// <summary>
        /// Sets a mask to the filter.
        /// </summary>
        /// <param name="mask">Mask to be set. Accepts only square masks with odd lenghts.</param>
        /// <returns>True if mask valid, false otherwise.</returns>
        public bool SetMask(int[,] mask)
        {
            bool outcome = false;
            if(mask != null)
            {
                int xSize, ySize;
                xSize = mask.GetLength(0);
                ySize = mask.GetLength(1);
                if(xSize == ySize && xSize%2 == 1)
                {
                    this.mask = (int[,])mask.Clone();
                    maskWeight = 0;
                    foreach(short s in this.mask)
                    {
                        maskWeight += s;
                    }
                    maskRadius = xSize/2;
                    outcome = true;
                }
            }
            return outcome;
        }

        /// <summary>
        /// Filters BitmapSource.
        /// </summary>
        /// <param name="bitmap">BitmapSource to be processed.</param>
        /// <returns>Filtered bitmap as BitmapSource.</returns>
        public BitmapSource Process(BitmapSource bitmap)
        {
            BitmapSource outcome = null;
            if (mask != null && bitmap != null)
            {
                int[,] values = BitmapHelper.GetValuesFromBitmapSource(bitmap);
                values = ProcessInt(values);
                outcome = BitmapHelper.GetBitmapSourceFromValues(values);
            }
            return outcome;
        }


        /// <summary>
        /// Apply filter on int[,] array.
        /// </summary>
        /// <param name="values">array to be processed.</param>
        /// <returns>int[,] array of processed values</returns>
        public int[,] ProcessInt(int[,] values)
        {
            int[,] outcome = null;
            if (mask != null && values != null)
            {
                outcome = new int[values.GetLength(0), values.GetLength(1)];
                Parallel.For(0, values.GetLength(0), x =>
                {
                    int xMinOff, xMaxOff;
                    xMinOff = Math.Max(-maskRadius, -x);
                    xMaxOff = Math.Min(values.GetLength(0) - x, maskRadius + 1);
                    for (int y = 0; y < values.GetLength(1); y++)
                    {
                        int sum = 0;
                        int  yMinOff, yMaxOff;
                        yMinOff = Math.Max(-maskRadius, -y);
                        yMaxOff = Math.Min(values.GetLength(1) - y, maskRadius + 1);
                        for (int xOff = xMinOff; xOff < xMaxOff; xOff++)
                        {
                            for (int yOff = yMinOff; yOff < yMaxOff; yOff++)
                            {
                                sum += values[x + xOff, y + yOff] * mask[maskRadius + xOff, maskRadius + yOff];
                            }
                        }
                        if (maskWeight == 0)
                        {
                            outcome[x, y] = Math.Max(0, Math.Min(255, sum + 127));
                        }
                        else
                        {
                            outcome[x, y] = Math.Max(0, Math.Min(255, sum / maskWeight));
                        }

                    }
                }
                    );
            }
            return outcome;
        }

    }
}

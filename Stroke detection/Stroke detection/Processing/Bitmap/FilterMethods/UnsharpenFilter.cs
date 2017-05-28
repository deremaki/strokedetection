using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.FilterMethods
{
    /// <summary>
    /// Unsharpen filter class.
    /// Implements IProcBitmap.
    /// Uses GaussFilter
    /// </summary>
    public class UnsharpenFilter : IProcBitmap
    {
        /// <summary>
        /// Smooth filter as GaussFitler.
        /// </summary>
        private GaussFilter smoothFilter;

        /// <summary>
        /// Unsharpens scaling constant
        /// </summary>
        private double scalingConstant;

        /// <summary>
        /// Creates new UnsharpenFilter with smooth filter as GaussFitler with mask of specified size.
        /// </summary>
        /// <param name="maskSize">Specifies size of Gauss Mask</param>
        /// <param name="scalingConstant">Specifies the scaling constant larger values provide increasing amount of sharpening</param>
        public UnsharpenFilter(int maskSize = 3, double scalingConstant = 1.0 )
        {
            smoothFilter = new GaussFilter(maskSize);
            this.scalingConstant = scalingConstant;
        }

        /// <summary>
        /// Filters BitmapSource with Unsharpenfilter.
        /// </summary>
        /// <param name="bitmap">BitmapSource to be processed.</param>
        /// <returns>Unsharpened bitmap as BitmapSource.</returns>
        public BitmapSource Process(BitmapSource bitmap)
        {
            BitmapSource smoothed = smoothFilter.Process(bitmap);
            int[,] baseValues = BitmapHelper.GetValuesFromBitmapSource(bitmap);
            int[,] smoothedValues = BitmapHelper.GetValuesFromBitmapSource(smoothed);
            int[,] outcomeValues = Unsharpen(baseValues, smoothedValues);
            return BitmapHelper.GetBitmapSourceFromValues(outcomeValues);
        }

        /// <summary>
        /// Usharpen the values from base values and smoothed values;
        /// </summary>
        /// <param name="baseValues">int[,] array of base values</param>
        /// <param name="smoothedValues">int[,] array of smoothed values</param>
        /// <returns>int[,] array of values after processing. Null if any of the parameters was null or their sizes were uneven.</returns>
        public int[,] Unsharpen(int[,] baseValues, int[,] smoothedValues)
        {
            int[,] outcome = null;
            if (baseValues != null && smoothedValues != null
                && baseValues.GetLength(0) == smoothedValues.GetLength(0) && baseValues.GetLength(1) == smoothedValues.GetLength(1))
            {
                outcome = new int[baseValues.GetLength(0), smoothedValues.GetLength(1)];
                Parallel.For(0, baseValues.GetLength(0), x =>
                {
                    for (int y = 0; y < baseValues.GetLength(1); y++)
                    {
                        int diff = Math.Min(0, Math.Max(255, baseValues[x, y] - smoothedValues[x, y]));
                        outcome[x, y] = Math.Min(0, Math.Max(255, baseValues[x, y] + (int)(scalingConstant * (double)diff)));
                    }
                });
            }
            return outcome;
        }

    }
}

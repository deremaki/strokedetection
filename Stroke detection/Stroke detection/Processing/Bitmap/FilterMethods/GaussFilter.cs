using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.FilterMethods
{
    /// <summary>
    /// Gaussian filter class.
    /// Implements IProcBitmap.
    /// </summary>
    public class GaussFilter : IProcBitmap
    {
        /// <summary>
        /// Filter used to Process.
        /// </summary>
        private BitmapFilter filter;
        
        /// <summary>
        /// Creates new instance of GaussFilter with mask of specified sizeby using one of prespecified masks.
        /// </summary>
        /// <param name="maskSize">Size of the mask.
        /// Supported sizes: 3, 5, 7
        /// Default size used in case size is not given or is invalid: 3</param>
        public GaussFilter(int maskSize = 3)
        {
            filter = new BitmapFilter();
            if (!filter.SetMask(getGaussMask(maskSize)))
                filter.SetMask(getGaussMask(3));
        }

        /// <summary>
        /// Sets mask of specified size, by using one of prespecified masks.
        /// </summary>
        /// <param name="maskSize">Size of new mask.
        /// Supported sizes: 3, 5, 7</param>
        /// <returns>True if mask changed, false otherwise.</returns>
        public bool SetMask(int maskSize)
        {
            bool outcome = false;
            int[,] mask = getGaussMask(maskSize);
            if(mask != null)
            {
                outcome = filter.SetMask(mask);
            }
            return outcome;
        }

        /// <summary>
        /// Returns one of prespecified masks.
        /// </summary>
        /// <param name="maskSize">Size of the mask.
        /// Supported sizes: 3, 5, 7</param>
        /// <returns>short[,] prespecified mask, null if invalid size</returns>
        private int[,] getGaussMask(int maskSize)
        {
            int[,] outcome = null;
            switch(maskSize)
            {
                case 3:
                    outcome = new int[,] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
                    break;
                case 5:
                    outcome = new int[,] { { 1, 1, 2, 1, 1 }, { 1, 2, 4, 2, 1 }, { 2, 4, 8, 4, 2 }, { 1, 2, 4, 2, 1 }, { 1, 1, 2, 1, 1 } };
                    break;
                case 7:
                    outcome = new int[,] { { 1, 1, 2, 2, 2, 1, 1 }, { 1, 2, 2, 4, 2, 2, 1 }, { 2, 2, 4, 8, 4, 2, 2 }, { 2, 4, 8, 16, 8, 4, 2 }, { 2, 2, 4, 8, 4, 2, 2 }, { 1, 2, 2, 4, 2, 2, 1 }, { 1, 1, 2, 2, 2, 1, 1 } };
                    break;
            }
            return outcome;
        }

        /// <summary>
        /// Filters BitmapSource with Gaussfilter.
        /// </summary>
        /// <param name="bitmap">BitmapSource to be processed.</param>
        /// <returns>Gauss filtered bitmap as BitmapSource.</returns>
        public BitmapSource Process(BitmapSource bitmap)
        {
            return filter.Process(bitmap);
        }
    }
}

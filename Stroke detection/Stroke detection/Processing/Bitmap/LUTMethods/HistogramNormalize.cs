using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.LUTMethods
{
    /// <summary>
    /// Class for histogram normalization.
    /// Implements IProcBitmap.
    /// </summary>
    public class HistogramNormalize : IProcBitmap
    {

        /// <summary>
        /// Processes BitmapSource to normalize histogram.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapSource Process(BitmapSource bitmap)
        {
            Histogram histogram = new Histogram();
            histogram.CreateHistogram(bitmap);
            LUTProcess lut = new LUTProcess();
            lut.SetLUT(CreateLUT(histogram.HistogramValues));
            return lut.Process(bitmap);
        }

        /// <summary>
        /// Creates LUT for histogram normalization.
        /// </summary>
        /// <param name="histogram">Histogram for normalization/</param>
        /// <returns>LUT table. null if histogram was invalid.</returns>
        public int[] CreateLUT(int[] histogram)
        {
            int[] outcome = null;
            if (histogram != null && histogram.Length == 256)
            {
                int min, max;
                min = max = -1;
                for (int i = 0; (min == -1 || max == -1) && i < 256; i++) 
                {
                    if (min == -1) 
                    {
                        if (histogram[i] != 0) 
                            min = i;
                    }
                    if (max == -1)
                    {
                        if (histogram[255 - i] != 0) 
                            max = i;
                    }
                }
                double val;
                if (min == max)
                {
                    min = 0;
                    val = 1;
                }
                else
                {
                    val = 255.0 / (double)(max - min);
                }
                outcome = new int[256];
                for(int i = 0; i< 256; i++)
                {
                    outcome[i] = Math.Min(255, Math.Max((int)((histogram[i] - min) * val), 0));
                }
            }
            return outcome;
        }
    }
}

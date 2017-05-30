using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.LUTMethods
{
    /// <summary>
    /// Class for histogram equalization.
    /// Implements IProcBitmap.
    /// </summary>
    public class HistogramEqualize : IProcBitmap
    {

        /// <summary>
        /// Processes BitmapSource to equalize histogram.
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
        /// Creates LUT for histogram equalization.
        /// </summary>
        /// <param name="histogram">Histogram for equalization/</param>
        /// <returns>LUT table. null if histogram was invalid.</returns>
        public int[] CreateLUT(int[] histogram)
        {
            int[] outcome = null;
            if(histogram != null && histogram.Length == 256)
            {
                outcome = new int[256];
                int total = histogram.Sum();
                int val = 0;
                double[] D = new double[256];
                double nonZero = -1;
                for (int i = 0; i < 256; i++)
                {
                    val += histogram[i];
                    D[i] = (double)val / (double)total;
                    if (nonZero == -1)
                        if (val != 0)
                            nonZero = D[i];
                }

                for (int i = 0; i < 256; i++)
                {
                    outcome[i] = (int)((D[i] - nonZero) * (255.0) / (1.0 - nonZero));
                }

            }
            return outcome;
        }
    }
}

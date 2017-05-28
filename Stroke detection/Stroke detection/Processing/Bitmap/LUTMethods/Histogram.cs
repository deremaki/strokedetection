using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap.LUTMethods
{
    /// <summary>
    /// Histogram class
    /// </summary>
    public class Histogram
    {
        /// <summary>
        /// Holds histogram Values
        /// </summary>
        private int[] histogramValues;

        /// <summary>
        /// Public access to histogram
        /// </summary>
        public int[] HistogramValues
        {
            get
            {
                return histogramValues;
            }

            set
            {
                histogramValues = value;
            }
        }

        /// <summary>
        /// Creates a histogram of specified BitmapSource with range of values from 0 to 255.
        /// </summary>
        /// <param name="bitmap">BitmapSource specified</param>
        /// <returns>True if creation was success, false otherwise.</returns>
        public bool CreateHistogram(BitmapSource bitmap)
        {
            bool outcome = false;
            if(bitmap != null)
            {
                outcome = CreateHistogramFromInt(BitmapHelper.GetValuesFromBitmapSource(bitmap));
            }
            return outcome; 
        }

        /// <summary>
        /// Creates a histogram of specified int[,]values with range of values from 0 to 255.
        /// </summary>
        /// <param name="values">int[,] specified.</param>
        /// <returns>True if creation was success, false otherwise.</returns>
        public bool CreateHistogramFromInt(int[,] values)
        {
            bool outcome = false;
            if(values != null)
            {
                int[] temp = new int[256];
                outcome = true;
                Parallel.For(0, values.GetLength(0), x =>
                {
                    for (int y = 0; outcome && y < values.GetLength(1); y++)
                    {
                        if (values[x, y] >= 0 && values[x, y] < 255)
                        {
                            temp[values[x, y]]++;
                        }
                        else
                        {
                            outcome = false;
                        }
                    }
                });
                if (outcome)
                    histogramValues = temp;
            }
            return outcome;
        }
    }
}

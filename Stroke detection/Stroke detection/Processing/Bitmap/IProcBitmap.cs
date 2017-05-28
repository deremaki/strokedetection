using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stroke_detection.Processing.Bitmap
{
    /// <summary>
    /// Processes a Bitmap
    /// </summary>
    public interface IProcBitmap
    {
        /// <summary>
        /// Processes specified BitmapSource into new BitmapSource.
        /// </summary>
        /// <param name="bitmap">BitmapSource to be processed.</param>
        /// <returns>Processed bitmap as BitmapSource.</returns>
        BitmapSource Process(BitmapSource bitmap);
    }
}

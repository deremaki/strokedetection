using System.Windows.Media.Imaging;

namespace Stroke_detection
{
    public class BitmapInfo
    {
        public BitmapSource Bitmap { get; set; }
        public int WindowMin { get; set; }
        public int WindowMax { get; set; }
        public string Filename { get; set; }
    }
}

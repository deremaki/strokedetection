using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stroke_detection
{
    /// <summary>
    /// Interaction logic for BitmapPreview.xaml
    /// </summary>
    public partial class BitmapPreview : Window
    {
        public BitmapPreview(BitmapSource bmp)
        {
            InitializeComponent();

            ImageBox.Source = bmp;
        }
    }
}

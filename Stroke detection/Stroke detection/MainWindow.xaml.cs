using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DicomImageViewer;
using Microsoft.Win32;

namespace Stroke_detection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DicomDecoder dd;

        private ushort[] FileData { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;

            dd = new DicomDecoder();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName.Length > 0)
                {
                    ImageBox.Source = ReadAndDisplayDicomFile(ofd.FileName, ofd.SafeFileName);
                }
            }
        }

        private BitmapSource ReadAndDisplayDicomFile(string fileName, string fileNameOnly)
        {
            dd.DicomFileName = fileName;

            List<ushort> pixels16 = new List<ushort>();
            int imageWidth;
            int imageHeight;
            int bitDepth;
            int samplesPerPixel;
            double winCentre;
            double winWidth;

            TypeOfDicomFile typeOfDicomFile = dd.typeofDicomFile;

            if (typeOfDicomFile == TypeOfDicomFile.Dicom3File || typeOfDicomFile == TypeOfDicomFile.DicomOldTypeFile)
            {
                imageWidth = dd.width;
                imageHeight = dd.height;
                bitDepth = dd.bitsAllocated;
                winCentre = dd.windowCentre;
                winWidth = dd.windowWidth;
                samplesPerPixel = dd.samplesPerPixel;

                if (samplesPerPixel == 1 && bitDepth == 16)
                {
                    pixels16.Clear();
                    dd.GetPixels16(ref pixels16);

                    int minPixelValue = pixels16.Min();
                    int maxPixelValue = pixels16.Max();

                    if (Math.Abs(winWidth) < 0.001)
                    {
                        winWidth = maxPixelValue - minPixelValue;
                    }

                    if ((winCentre == 0) ||
                        (minPixelValue > winCentre) || (maxPixelValue < winCentre))
                    {
                        winCentre = (maxPixelValue + minPixelValue) / 2;
                    }

                    return ToBitmapImage(pixels16.ToArray(), imageWidth, imageHeight, winWidth, winCentre);
                }

                else
                {
                    MessageBox.Show("Unsupported bits per pixel number (only 16bpp supported)");
                }

            }
            else
            {
                if (typeOfDicomFile == TypeOfDicomFile.DicomUnknownTransferSyntax)
                {
                    MessageBox.Show("Invalid dicom file");
                }
                else
                {
                    MessageBox.Show("Invalid file");
                }

                return null;
            }
            return null;
        }

        private static BitmapSource ToBitmapImage(ushort[] imageData, int width, int height, double windowWidth,
        double windowCentre)
        {
            int rawStride = width * 2;

            int sizeImg = width * height;
            int sizeImg3 = sizeImg * 1;
            var imagePixels16 = new byte[sizeImg3];
            byte[] lut16 = new byte[65536];

            int winMax = Convert.ToInt32(windowCentre + 0.5 * windowWidth);
            int winMin = winMax - Convert.ToInt32(windowWidth);

            int range = winMax - winMin;
            if (range < 1) range = 1;
            double factor = 255.0 / range;

            for (int i = 0; i < 65536; ++i)
            {
                if (i <= winMin)
                    lut16[i] = 0;
                else if (i >= winMax)
                    lut16[i] = 255;
                else
                {
                    lut16[i] = (byte)((i - winMin) * factor);
                }
            }

            int pixelSize = 1;
            int j, j1;
            byte b;

            List<byte> list = new List<byte>();

            foreach (var pixel in imageData)
            {
                list.Add(lut16[pixel]);
            }

            var buffer8Bit = list.ToArray();

            return BitmapSource.Create(width, height,
                    300, 300, PixelFormats.Gray8, BitmapPalettes.Gray256,
                    buffer8Bit, rawStride / 2);
        }

    }
}

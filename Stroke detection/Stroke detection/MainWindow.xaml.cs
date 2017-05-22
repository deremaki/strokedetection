using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
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
                    ImageBox.Source = ReadDicomFile(ofd.FileName, ofd.SafeFileName);
                }
            }
        }

        private BitmapSource ReadDicomFile(string fileName, string fileNameOnly)
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

                    return BitmapHelper.ToBitmapImage(pixels16.ToArray(), imageWidth, imageHeight, winWidth, winCentre);
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

        

    }
}

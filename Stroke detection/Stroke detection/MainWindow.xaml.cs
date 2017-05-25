﻿using System;
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

        private BitmapSource Bitmap
        {
            get { return Layers[(int)LayerSlider.Value].Bitmap; }
        }

        private BitmapInfo[] Layers;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            dd = new DicomDecoder();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "DICOM files (*.dcm)|*.dcm";
            if (ofd.ShowDialog() == true)
            {
                List<BitmapInfo> list = new List<BitmapInfo>();

                foreach (var filename in ofd.FileNames)
                {
                    if (!filename.EndsWith(".dcm"))
                    {
                        MessageBox.Show("One of the files is not a dicom file");
                        return;
                    }
                    int winMin;
                    int winMax;
                    list.Add(new BitmapInfo()
                    {
                        Bitmap = ReadDicomFile(filename, out winMin, out winMax),
                        WindowMax = winMax,
                        WindowMin = winMin
                    });
                }

                Layers = list.ToArray();

                LayerSlider.Minimum = 0;
                LayerSlider.Maximum = list.Count - 1;
                LayerSlider.Value = 0;
                LayerSlider.IsEnabled = true;

                ImageBox.Source = Layers[(int)LayerSlider.Value].Bitmap;
            }
        }

        private BitmapSource ReadDicomFile(string fileName, out int minWin, out int maxWin)
        {
            dd.DicomFileName = fileName;

            List<ushort> pixels16 = new List<ushort>();
            int imageWidth;
            int imageHeight;
            int bitDepth;
            int samplesPerPixel;
            double winCentre;
            double winWidth;

            maxWin = 0;
            minWin = 0;

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

                    maxWin = Convert.ToInt32(winCentre + 0.5 * winWidth);
                    minWin = maxWin - Convert.ToInt32(winWidth);

                    return BitmapHelper.ToBitmapImage(pixels16.ToArray(), imageWidth, imageHeight, minWin, maxWin);
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

        private void IMethodButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImageBox.Source == null)
                return;

            //processing image

            new BitmapPreview(Layers[(int)LayerSlider.Value].Bitmap).Show();
        }

        private void LayerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ImageBox.Source = Layers[(int)LayerSlider.Value].Bitmap;
        }
    }
}

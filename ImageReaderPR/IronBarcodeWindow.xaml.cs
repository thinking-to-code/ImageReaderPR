using IronBarCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for IronBarcodeWindow.xaml
    /// </summary>
    public partial class IronBarcodeWindow : Window
    {
        public IronBarcodeWindow()
        {
            InitializeComponent();
        }

        private Bitmap _selectedImage;

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Open File Dialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Load selected image
                _selectedImage = new Bitmap(openFileDialog.FileName);

                // Display image in the UI
                InputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void DecodeBarcode_Click(object sender, RoutedEventArgs e)
        {
            BarcodeResults results = BarcodeReader.Read(_selectedImage);
            if (results != null)
            {
                foreach (BarcodeResult result in results)
                {
                    BarcodeResult.Text = result.Text;
                }
            }

            //if (_selectedImage == null)
            //{
            //    MessageBox.Show("Please select an image first.");
            //    return;
            //}

            // Decode barcode using ZXing
            //var reader = new ZXing.Windows.Compatibility.BarcodeReader();
            //reader.Options = new ZXing.Common.DecodingOptions
            //{
            //    PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.PDF_417 }
            //};
            //var result = reader.Decode(_selectedImage);

            //if (result != null)
            //{
            //    BarcodeResult.Text = result.Text; // Display decoded text
            //}
            //else
            //{
            //    MessageBox.Show("No barcode detected in the image.");
            //}
        }
    }
}

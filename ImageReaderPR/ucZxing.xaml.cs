using ImageReaderPR.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for ucZxing.xaml
    /// </summary>
    public partial class ucZxing : UserControl
    {
        public ucZxing()
        {
            InitializeComponent();
        }

        private Bitmap _selectedImage;
        private string filePath;
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
                filePath = openFileDialog.FileName;

                // Display image in the UI
                InputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void DecodeBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            // Preprocess the image
            Bitmap processedImage = ImageHelper.PreprocessImage(_selectedImage);

            // Decode the barcode
            // Convert BitmapImage to Bitmap
            BitmapImage bitmapImage = new BitmapImage(new Uri(filePath));
            Bitmap bitmap = ImageHelper.BitmapFromBitmapImage(bitmapImage);

            // Convert Bitmap to byte array
            byte[] rgbBytes = ImageHelper.GetRGBBytesFromBitmap(bitmap);

            // Create LuminanceSource
            LuminanceSource source = new RGBLuminanceSource(rgbBytes, bitmap.Width, bitmap.Height);

            // Create BarcodeReader
            BarcodeReader<Result> reader = new BarcodeReader<Result>(t => source);
            reader.Options = new ZXing.Common.DecodingOptions
            {
                PossibleFormats = new[] {BarcodeFormat.PDF_417}
            };

            // Decode the barcode
            
            var result = reader.Decode(processedImage);

            if (result != null)
            {
                BarcodeResult.Text = result.Text; // Display decoded text
            }
            else
            {
                MessageBox.Show("No PDF417 barcode detected.");
            }
        }
    }
}

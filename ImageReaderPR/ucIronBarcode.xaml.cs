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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for ucIronBarcode.xaml
    /// </summary>
    public partial class ucIronBarcode : UserControl
    {
        public ucIronBarcode()
        {
            InitializeComponent();

            IronBarCode.License.LicenseKey = "IRONSUITE.MUHAMMADIQBAL.EXCEEDGULF.COM.27754-A32CAE2734-CSF3WX5-MQSNU3YEGTIY-XHUS7WYEWTY5-XNNFZWDE7BB7-W2EUWYKDLMMQ-7D47X2MGQW35-PA633WYXCKVR-HPVJC3-TPJIFAYJSR2OUA-DEPLOYMENT.TRIAL-26NRVM.TRIAL.EXPIRES.06.MAR.2025";

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
            // To configure and fine-tune barcode reading, utilize the BarcodeReaderOptions class
            var myOptionsExample = new BarcodeReaderOptions
            {
                // Choose a reading speed from: Faster, Balanced, Detailed, ExtremeDetail
                // There is a tradeoff in performance as more detail is set
                Speed = ReadingSpeed.Balanced,

                // Reader will stop scanning once a single barcode is found (if set to true)
                ExpectMultipleBarcodes = true,

                // By default, all barcode formats are scanned for
                // Specifying a subset of barcode types to search for would improve performance
                ExpectBarcodeTypes = BarcodeEncoding.PDF417,

                // Utilize multiple threads to read barcodes from multiple images in parallel
                Multithreaded = true,

                // Maximum threads for parallelized barcode reading
                // Default is 4
                MaxParallelThreads = 2,

                // The area of each image frame in which to scan for barcodes
                // Specifying a crop area will significantly improve performance and avoid noisy parts of the image
                //CropArea = new Rectangle(),

                // Special setting for Code39 barcodes
                // If a Code39 barcode is detected, try to read with both the base and extended ASCII character sets
                UseCode39ExtendedMode = true
            };
            BarcodeResults results = BarcodeReader.Read(_selectedImage, myOptionsExample);
            if (results != null && results.Count > 0)
            {
                foreach (BarcodeResult result in results)
                {
                    BarcodeResult.Text = result.Text;
                }
            }
            else
            {
                MessageBox.Show("No barcode detected in the image.");
            }            
        }
    }
}

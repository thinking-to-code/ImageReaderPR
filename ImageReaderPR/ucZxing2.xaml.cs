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
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ZXing;

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for ucZxing2.xaml
    /// </summary>
    public partial class ucZxing2 : UserControl
    {
        public ucZxing2()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBlock.Text = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                ImagePreview.Source = bitmapImage;

                try
                {
                    // Convert BitmapImage to Bitmap
                    Bitmap bitmap = ImageHelper.BitmapFromBitmapImage(bitmapImage);

                    // Convert Bitmap to byte array
                    byte[] rgbBytes = ImageHelper.GetRGBBytesFromBitmap(bitmap);

                    // Create LuminanceSource
                    LuminanceSource source = new RGBLuminanceSource(rgbBytes, bitmap.Width, bitmap.Height);

                    // Create BarcodeReader
                    BarcodeReader<Result> reader = new BarcodeReader<Result>(t => source);

                    // Decode the barcode
                    Result result = reader.Decode(bitmap);

                    if (result != null)
                    {
                        ResultTextBlock.Text = $"Decoded Value: {result.Text}";
                    }
                    else
                    {
                        ResultTextBlock.Text = "No barcode found.";
                    }
                }
                catch (Exception ex)
                {
                    ResultTextBlock.Text = $"Error: {ex.Message}";
                }
            }
        }

    }
}

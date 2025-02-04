using IronBarCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using ZXing;

namespace ImageReaderPR.Helper
{
    public class ZXingBarcodeReaderService
    {
        public string DecodeBarcode(Bitmap _selectedImage, string filePath)
        {
            if (_selectedImage == null)
            {
                return "Please load an image first.";                
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
                PossibleFormats = new[] { BarcodeFormat.PDF_417 }
            };

            // Decode the barcode

            var result = reader.Decode(processedImage);

            if (result != null)
            {
                return result.Text; // Display decoded text
            }
            
            return "No PDF417 barcode detected.";            
        }
    }
}

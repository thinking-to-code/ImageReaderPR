﻿using Dynamsoft.Core;
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
using Dynamsoft.License;
using Dynamsoft.CVR;
using Dynamsoft.DBR;

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for ucDynamoBarcode.xaml
    /// </summary>
    public partial class ucDynamoBarcode : UserControl
    {
        public ucDynamoBarcode()
        {
            InitializeComponent();
            int errorCode = 1;
            string errorMsg;

            // Initialize license.
            // You can request and extend a trial license from https://www.dynamsoft.com/customer/license/trialLicense?product=dbr&utm_source=samples&package=dotnet
            // The string 't0073oQAAAFHMK2bCXtn4ZfUyVLW/khBUWBb176MtWPRcILOyHzVCogCJWtJL8l3TWkMclbo8/ZR2UVqDF2NwICQQ1ciDTVQBNQAjig==' here is a free public trial license. Note that network connection is required for this license to work.
            errorCode = LicenseManager.InitLicense("t0074oQAAAHt3mSY7eT1eeJK+MHrfmPMljwuqBHZ4Yim76CO9y6JhHs7f85LZ8HxrQa2hkdAywZZY9zh/wc3HMEURgnQoDwPLGzWpI4s=", out errorMsg);

            if (errorCode != (int)EnumErrorCode.EC_OK && errorCode != (int)EnumErrorCode.EC_LICENSE_CACHE_USED)
            {
                MessageBox.Show("License initialization failed: ErrorCode: " + errorCode + ", ErrorString: " + errorMsg);
            }
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
            BarcodeResult.Text = "";
            if (_selectedImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }
            //
            
            using (CaptureVisionRouter cvr = new CaptureVisionRouter())
            {
                SimplifiedCaptureVisionSettings settings;
                string errorMsg;
                // Obtain current runtime settings of `CCaptureVisionRouter` instance.
                cvr.GetSimplifiedSettings(PresetTemplate.PT_READ_BARCODES, out settings);
                // Specify the barcode formats by enumeration values.
                // Use "|" to enable multiple barcode formats at one time.
                settings.barcodeSettings.barcodeFormatIds = (ulong)(EnumBarcodeFormat.BF_PDF417 | EnumBarcodeFormat.BF_MICRO_PDF417);
                // Update the settings.
                cvr.UpdateSettings(PresetTemplate.PT_READ_BARCODES, settings, out errorMsg);

                //string imageFile = "../../../../../../Images/GeneralBarcodes.png";
                string imageFile = filePath;
                CapturedResult? result = cvr.Capture(imageFile, PresetTemplate.PT_READ_BARCODES);
                if (result == null)
                {
                    MessageBox.Show("No barcode detected.");
                }
                else
                {
                    if (result.GetErrorCode() != 0)
                    {
                        MessageBox.Show("Error: " + result.GetErrorCode() + ", " + result.GetErrorString());
                    }
                    StringBuilder sb = new StringBuilder();
                    DecodedBarcodesResult? barcodesResult = result.GetDecodedBarcodesResult();
                    if (barcodesResult != null)
                    {
                        BarcodeResultItem[] items = barcodesResult.GetItems();
                        sb.AppendLine("Decoded " + items.Length + " barcodes");
                        foreach (BarcodeResultItem barcodeItem in items)
                        {
                            sb.AppendLine("Result " + (Array.IndexOf(items, barcodeItem) + 1));
                            sb.AppendLine("Barcode Format: " + barcodeItem.GetFormatString());
                            sb.AppendLine("Barcode Text: " + barcodeItem.GetText());
                        }

                        BarcodeResult.Text = sb.ToString();
                    }
                }
            }            
        }
    }
}

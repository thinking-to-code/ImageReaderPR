using DocumentFormat.OpenXml.Drawing;
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
using System.Windows.Shapes;
using ZXing;

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for CombinedResultWindow.xaml
    /// </summary>
    public partial class CombinedResultWindow : Window
    {
        private List<ExcelDataObject> dataForExcel;
        private int count = 1;
        public CombinedResultWindow()
        {
            InitializeComponent();
            dataForExcel = new List<ExcelDataObject>();
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
            
            ZXingBarcodeReaderService zXingBarcodeReaderService = new ZXingBarcodeReaderService();
            ZXingBarcodeResult.Text = zXingBarcodeReaderService.DecodeBarcode(_selectedImage, filePath);

            ExcelDataObject zxDObj = new ExcelDataObject();
            zxDObj.Id = count;
            zxDObj.Vendor = Vendor.ZXing.ToString();
            zxDObj.ImagePath = filePath;
            zxDObj.DecodeResult = ZXingBarcodeResult.Text;
            dataForExcel.Add(zxDObj);

            IronBarcodeReaderService ironBarcodeReaderService = new IronBarcodeReaderService();
            IronBarcodeResult.Text = ironBarcodeReaderService.DecodeBarcode(_selectedImage, filePath);

            ExcelDataObject ironDObj = new ExcelDataObject();
            ironDObj.Id = count;
            ironDObj.Vendor = Vendor.IronBarcode.ToString();
            ironDObj.ImagePath = filePath;
            ironDObj.DecodeResult = IronBarcodeResult.Text;
            dataForExcel.Add(ironDObj);

            DynamsoftBarcodeReaderService dynamsoftBarcodeReaderService = new DynamsoftBarcodeReaderService();
            DynamBarcodeResult.Text = dynamsoftBarcodeReaderService.DecodeBarcode(_selectedImage, filePath);

            ExcelDataObject dynamDObj = new ExcelDataObject();
            dynamDObj.Id = count;
            dynamDObj.Vendor = Vendor.Dynamsoft.ToString();
            dynamDObj.ImagePath = filePath;
            dynamDObj.DecodeResult = DynamBarcodeResult.Text;
            
            dataForExcel.Add(dynamDObj);
            
            count++;
        }

        private void btnExcelExport_Click(object sender, RoutedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // Format: 20240204_153045
            string fileName = $"BarcodeDecodeResults_{timestamp}.xlsx";
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            ExcelExporter.ExportToExcel(dataForExcel, filePath);

            MessageBox.Show($"File exported at path: {filePath}");
        }
    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // For file dialog
using System.Drawing; // For Bitmap
using System.Windows.Media.Imaging; // For BitmapImage
using ZXing; // For barcode decoding

namespace ImageReaderPR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap _selectedImage;
        public MainWindow()
        {
            InitializeComponent();
        }

        
    }
}
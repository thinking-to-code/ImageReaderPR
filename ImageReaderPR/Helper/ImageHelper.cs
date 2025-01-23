using Emgu.CV.CvEnum;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using ZXing;
using System.IO;
using System.Drawing.Imaging;

namespace ImageReaderPR.Helper
{
    public static class ImageHelper
    {
        public static Bitmap PreprocessImage(Bitmap bitmap)
        {
            // Convert Bitmap to Mat for processing
            //Mat mat = BitmapToMat(bitmap);

            // Resize to standardize dimensions
            //CvInvoke.Resize(mat, mat, new System.Drawing.Size(800, 800), 0, 0, Inter.Linear);

            // Convert to grayscale
            //CvInvoke.CvtColor(mat, mat, ColorConversion.Bgr2Gray);
            bitmap = ConvertToGrayscale(bitmap);

            // Apply Gaussian blur to reduce noise
            //CvInvoke.GaussianBlur(mat, mat, new System.Drawing.Size(5, 5), 1);

            // Enhance contrast with histogram equalization
            //CvInvoke.EqualizeHist(mat, mat);

            // Deskew (optional)
            // Additional deskewing logic can be applied here if needed.

            // Convert back to Bitmap for ZXing
            //return mat.ToBitmap();
            return bitmap;
        }

        private static Mat BitmapToMat(Bitmap bitmap)
        {
            // Convert Bitmap to byte array
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = stream.ToArray();

                // Decode the byte array to a Mat
                Mat mat = new Mat();
                CvInvoke.Imdecode(imageBytes, ImreadModes.Color, mat);
                return mat;
            }
        }

        // Extension method to convert BitmapImage to Bitmap
        public static Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                return new System.Drawing.Bitmap(stream);
            }
        }

        // Helper function to convert BitmapImage to Bitmap
        public static Bitmap BitmapFromBitmapImage(BitmapImage bitmapImage)
        {
            using (var stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                stream.Position = 0;
                return new Bitmap(stream);
            }
        }

        // Helper function to convert Bitmap to Grayscale
        public static Bitmap ConvertToGrayscale(Bitmap original)
        {
            Bitmap grayBitmap = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(grayBitmap))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                        new float[] {0.299f, 0.587f, 0.114f, 0, 0},
                        new float[] {0.299f, 0.587f, 0.114f, 0, 0},
                        new float[] {0.299f, 0.587f, 0.114f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                    0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return grayBitmap;
        }

        // Helper function to get RGB bytes from Bitmap
        public static byte[] GetRGBBytesFromBitmap(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte[] rgbBytes = new byte[bitmapData.Stride * bitmapData.Height];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, rgbBytes, 0, rgbBytes.Length);
            bitmap.UnlockBits(bitmapData);
            return rgbBytes;
        }        
    }
}

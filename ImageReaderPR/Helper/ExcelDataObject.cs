using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageReaderPR.Helper
{
    public enum Vendor
    {
        ZXing = 1,
        IronBarcode,
        Dynamsoft
    }
    public class ExcelDataObject
    {
        public int Id { get; set; }
        public string Vendor { get; set; }
        public string ImagePath { get; set; }
        public string DecodeResult { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WildPay.Tools
{
    public class ConverterTools
    {
        private static ConverterTools instance = new ConverterTools();
        private ConverterTools() { }

        public static ConverterTools Instance
        {
            get { return instance; }
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
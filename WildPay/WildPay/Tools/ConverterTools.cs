using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WildPay.Tools
{
    public static class ConverterTools
    {
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static byte[] FileToByteArray(HttpPostedFileBase newImage)
        {
            MemoryStream target = new MemoryStream();
            newImage.InputStream.CopyTo(target);
            return target.ToArray();
        }

        public static string ByteArrayToStringImage(byte[] byteImage)
        {
            var base64 = Convert.ToBase64String(byteImage);
            return String.Format("data:image/gif;base64,{0}", base64);
        }
        
    }
}
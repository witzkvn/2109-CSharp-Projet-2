using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WildPay.Tools
{
    public class AssetsTools
    {
        static string GetDefaultUserImageFilePath()
        {
            string path = "Content\\Images";
            string combinedPath = Path.Combine(path, "default.png");
            return combinedPath;
        }

        public static Image GetDefaultUserImageFile()
        {
            return Image.FromFile(GetDefaultUserImageFilePath());
        }
    }
}
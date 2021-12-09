using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WildPay.Tools
{
    public class FormatTools
    {
        private static string LettresAZAccent { get {
                return "A-Za-zàèìòùÀÈÌÒÙáéíóúýÁÉÍÓÚÝâêîôûÂÊÎÔÛãñõÃÑÕäëïöüÿÄËÏÖÜŸçÇßØøÅåÆæœ";
            } }
        public static string RegexText { get {
                return "^[" + LettresAZAccent + "][" + LettresAZAccent + @"\\ \\-]*[" + LettresAZAccent + "]$";
            } }
        public static string RegexTextAndNumbers
        {
            get
            {
                return "^[0-9" + LettresAZAccent + "][0-9" + LettresAZAccent + @"\\ \\-]*[0-9" + LettresAZAccent + "]$";
            }
        }
        public static string RegexPassword
        {
            get
            {
                return @"(?=.*\d)(?=.*[0-9])(?=.*[^A-Za-z0-9]).{5,}";
            }
        }

        public static bool IsPasswordFormatOk(string userPassword)
        {
            // au moins 1 chiffre 
            // au moins 1 caractère special 
            // au moins 5 caractères de long
            return new Regex(RegexPassword).IsMatch(userPassword);
        }

        public static bool IsTextOk(string text)
        {
            // que des lettres et lettres avec accents
            return new Regex(RegexText).IsMatch(text);
        }

        public static bool IsTextAndNumberOk(string textAndNumbers)
        {
            // que des lettres et lettres avec accents
            return new Regex(RegexTextAndNumbers).IsMatch(textAndNumbers);
        }

        public static bool IsDateOk(string date)
        {
            DateTime dateResult;
            if (DateTime.TryParse(date, out dateResult))
            {
                if (dateResult > DateTime.Now)
                    return false;
                else if (dateResult.Year < DateTime.Now.Year - 10)
                    return false;
            }
            return false;
        }

        public static string ConvertInShortDate(DateTime date)
        { 
            return date.ToString("dd/MM/yyyy"); ;
        }



        public static string HashPassword(string passwordToHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(passwordToHash);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                Console.WriteLine("hash : " + sb.ToString());
                return sb.ToString();
            }
        }

        public static bool VerifyImageFormatAndSize(HttpPostedFileBase newImage)
        {
            if(newImage.ContentLength < 2000000 && (newImage.ContentType == "image/jpg" || newImage.ContentType == "image/jpeg" || newImage.ContentType == "image/png"))
            {
                return true;
            }
            return false;
        }

        public static double ConvertinShortDouble(double shortDouble)
        {
            return Math.Round(shortDouble, 2);
        }
    }
}
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
        public static bool IsPasswordFormatOk(string userPassword)
        {
            // au moins 1 chiffre 
            // au moins 1 caractère special 
            // au moins 5 caractères de long
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex specialChars = new Regex("[^A-Za-z0-9]");

            return
                hasNumber.IsMatch(userPassword) &&
                userPassword.Length >= 5 &&
                specialChars.IsMatch(userPassword);
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
    }
}
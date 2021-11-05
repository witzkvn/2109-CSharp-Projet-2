using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WildPay.Tools
{
    public class FormatTools
    {
        public static bool IsPasswordFormatOk(string userPassword)
        {
            // a implémenter 
            // regex ? 
            // au moins 1 chiffre
            // au moins un caractère spécial
            // au moins 5 caractères de long
            return true;
        }

        public static bool IsDateOk(string date)
        {
            // Verifie format de date 
            // utilisé pour entrer une dépense
            return true;
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
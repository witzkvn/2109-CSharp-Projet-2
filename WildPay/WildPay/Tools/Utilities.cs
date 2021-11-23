using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WildPay.Models;

namespace WildPay.Tools
{
    public static class Utilities
    {
        public static string GetPremiereLettreMajuscule(string mot)
        {
            if (mot.Length == 0)
            {
                return mot;
            }
            else if (mot.Length == 1)
            {
                return "" + char.ToUpper(mot[0]);
            }
            else
            {
                return char.ToUpper(mot[0]) + mot.Substring(1);
            }
        }

        public static string GetNomCompletUser(User user)
        {
            return $"{user.Lastname.ToUpper()} {GetPremiereLettreMajuscule(user.Firstname)}";
        }
    }
}
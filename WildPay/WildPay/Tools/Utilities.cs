using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WildPay.DAL;
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
            if (user != null)
                return $"{user.Lastname.ToUpper()} {GetPremiereLettreMajuscule(user.Firstname)}";
            else return "Utilisateur WildPay";
        }

        public static string GetNomCompletUser(ListUser user)
        {
            if (user != null)
                return $"{user.Lastname.ToUpper()} {GetPremiereLettreMajuscule(user.Firstname)}";
            else return "Utilisateur WildPay";
        }

        public static int GetGroupePrincipalId()
        {
            int principalGroupId = 0;
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                principalGroupId = (int)returnCode.Value;
            }
            return principalGroupId;
        }




    }
}
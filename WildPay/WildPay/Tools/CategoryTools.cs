using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using WildPay.DAL;
using WildPay.Models;

namespace WildPay.Tools
{
    public class CategoryTools
    {
        public static List<Category> GetCategoriesForDefaultGroup()
        {
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                var type = returnCode.Value.GetType().FullName;

                if (type != "System.DBNull")
                {
                    int groupId = (int)returnCode.Value;
                    List<Category> categories = db.Database.SqlQuery<Category>
                    ("sp_GetCategory @p0", groupId)
                    .ToList();
                    return categories;
                }
                return null;
            }
        }
    }
}
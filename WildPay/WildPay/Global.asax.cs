using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WildPay.DAL;
using WildPay.Models;

namespace WildPay
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            #region Recup procedures stockees
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WildPay.BDD.ProcedureStockee.sql";

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            string[] commandesSql = result.Split(new string[] { " GO " }, StringSplitOptions.RemoveEmptyEntries);
            #endregion




            using (WildPayContext db = new WildPayContext())
            {
                foreach (var item in commandesSql)
                {
                    string newItem = item.Replace("\r\n", "");
                    if(newItem != "")
                    {
                        db.Database.ExecuteSqlCommand(item);
                    }
                }
                

                //var returnCode = new SqlParameter();
                //returnCode.ParameterName = "@GroupID ";
                //returnCode.SqlDbType = SqlDbType.Int;
                //returnCode.Direction = ParameterDirection.Output;

                //int groupeDefaut = db.Database.ExecuteSqlCommand("sp_CheckGroupePrincipal @GroupID OUTPUT", returnCode);

                //int idGroupePrincipal = -1;

                //if (returnCode.SqlValue.GetType().DeclaringType == null)
                //{
                //    var newGroupId = new SqlParameter();
                //    newGroupId.ParameterName = "@GroupID ";
                //    newGroupId.SqlDbType = SqlDbType.Int;
                //    newGroupId.Direction = ParameterDirection.Output;

                //    db.Database.ExecuteSqlCommand("sp_CreerGroup @name, @GroupID OUTPUT", 
                //            newGroupId,
                //            new SqlParameter("@name", "principal")
                //            );

                //    int groupId = (int)newGroupId.SqlValue;

                    //db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id",
                    //        new SqlParameter("@name", "restaurant"),
                    //        new SqlParameter("@name", groupId));
                    //db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id",
                    //        new SqlParameter("@name", "transport"),
                    //        new SqlParameter("@name", (int)newGroupId.SqlValue));
                    //db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id",
                    //        new SqlParameter("@name", "courses"),
                    //        new SqlParameter("@name", (int)newGroupId.SqlValue));
                    //db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id",
                    //        new SqlParameter("@name", "hebergement"),
                    //        new SqlParameter("@name", (int)newGroupId.SqlValue));
                //} 
            }
        }
    }
}

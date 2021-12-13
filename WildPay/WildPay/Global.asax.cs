using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Management;
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

            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new DoubleModelBinder());


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

                #region Creation groupe principal
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                var type = returnCode.Value.GetType().FullName;

                if (type == "System.DBNull")
                {
                    var newGroupId = new SqlParameter();
                    newGroupId.ParameterName = "@GroupID ";
                    newGroupId.SqlDbType = SqlDbType.Int;
                    newGroupId.Direction = ParameterDirection.Output;

                    db.Database.ExecuteSqlCommand("sp_CreerGroupInit @name, @GroupID OUTPUT",
                            newGroupId,
                            new SqlParameter("@name", "principal")
                            );

                    int groupId = (int)newGroupId.Value;

                    db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                            new SqlParameter("@name", "restaurant"),
                            new SqlParameter("@group_Id", groupId),
                            new SqlParameter("@IsBase", true));
                    db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                            new SqlParameter("@name", "transport"),
                            new SqlParameter("@group_Id", groupId),
                            new SqlParameter("@IsBase", true));
                    db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                            new SqlParameter("@name", "courses"),
                            new SqlParameter("@group_Id", groupId),
                            new SqlParameter("@IsBase", true));
                    db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                            new SqlParameter("@name", "hebergement"),
                            new SqlParameter("@group_Id", groupId),
                            new SqlParameter("@IsBase", true));
                }
                #endregion
            }
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;
            if (httpException == null) return;

            if (Request.FilePath == "/Account" && httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
            {
                Response.Redirect("/Account/Error");
            }
            else
            {
                Response.Redirect("/Home/Error");
            }
        }
    }
}

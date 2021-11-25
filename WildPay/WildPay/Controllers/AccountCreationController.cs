using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.DAL;
using WildPay.Models;
using WildPay.Tools;

namespace WildPay.Controllers
{
    public class AccountCreationController : Controller
    {
        public AccountCreationController()
        {
            ViewBag.Message ="";
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User newUser)
        {
            if (ModelState.IsValid)
            {
                using (WildPayContext db = new WildPayContext())
                {
                    bool userExists = db.Database.SqlQuery<User>("sp_GetUserByEmail @p0", newUser.Email.Trim().ToLower()).Any();

                    if (userExists)
                    {
                        ViewBag.Message = "Email déjà enregistré ! ";
                        return View("Index");
                    }

                    if (FormatTools.IsPasswordFormatOk(newUser.Password))
                    {
                        newUser.Password = FormatTools.HashPassword(newUser.Password.Trim());

                        var returnCode = new SqlParameter();
                        returnCode.ParameterName = "@GroupID ";
                        returnCode.SqlDbType = SqlDbType.Int;
                        returnCode.Direction = ParameterDirection.Output;

                        db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                        int principalGroupId = (int)returnCode.Value;


                        db.Database.ExecuteSqlCommand("sp_CreerUser @firstname, @lastname, @email, @password, @GroupID",
                            new SqlParameter("@firstname", newUser.Firstname.Trim()),
                            new SqlParameter("@lastname", newUser.Lastname.Trim()),
                            new SqlParameter("@email", newUser.Email.Trim().ToLower()),
                            new SqlParameter("@password", newUser.Password),
                            new SqlParameter("@GroupID", principalGroupId));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Format mot de passe incorrect ";
                    }

                }

            }
            return View("Index");
        }

    }
}
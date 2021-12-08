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

                        int principalGroupId = Utilities.GetGroupePrincipalId();


                        db.Database.ExecuteSqlCommand("sp_CreerUser @firstname, @lastname, @email, @password, @GroupID",
                            new SqlParameter("@firstname", newUser.Firstname.Trim()),
                            new SqlParameter("@lastname", newUser.Lastname.Trim()),
                            new SqlParameter("@email", newUser.Email.Trim().ToLower()),
                            new SqlParameter("@password", newUser.Password),
                            new SqlParameter("@GroupID", principalGroupId));

                        return RedirectToAction("Index", "Connexion", new {
                            creationSuccess = true
                    });
                    }
                    else
                    {
                        ViewBag.Message = "Mot de passe invalide. Utilisez au moins cinq caractères avec des majuscules, minuscules, des chiffres et des symboles.";
                    }

                }

            }
            return View("Index");
        }

    }
}
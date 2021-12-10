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
        public ActionResult Index(string error = null)
        {
            if(error != null)
            {
                ViewBag.Message = error;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(User newUser)
        {
            if (!FormatTools.IsTextOk(newUser.Firstname) || !FormatTools.IsTextOk(newUser.Lastname))
            {
                string updateMessage = "Le nom ou le prénom ne doivent contenir que des lettres.";
                return RedirectToAction("Index", "AccountCreation", new { error = updateMessage });
            }
            if (!FormatTools.IsPasswordFormatOk(newUser.Password))
            {
                string updateMessage = "Le mot de passe doit contenir au moins 5 caractères, dont 1 chiffre et 1 caractère spécial.";
                return RedirectToAction("Index", "AccountCreation", new { error = updateMessage });
            }
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

            }
            return View("Index");
        }

    }
}
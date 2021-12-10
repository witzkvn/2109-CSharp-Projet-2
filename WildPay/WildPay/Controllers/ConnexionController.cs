using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.DAL;
using WildPay.Models;
using WildPay.Tools;

namespace WildPay.Controllers
{
    public class ConnexionController : Controller
    {
        public ActionResult Index(bool creationSuccess = false)
        {
            ViewBag.ConfirmationCreationCompte = creationSuccess;
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(User userModel)
        {
            using (WildPayContext db = new WildPayContext())
            {
                string hashPassword = FormatTools.HashPassword(userModel.Password.Trim());
                User userDetails = db.Users.Where(x => x.Email.ToLower() == userModel.Email.ToLower() && x.Password.Trim() == hashPassword).FirstOrDefault();
                if(userDetails==null)
                {
                    userModel.loginErrorMessage = "Email ou mot de passe incorrects.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["Id"] = userDetails.Id;
                    Session["Firstname"] = Utilities.GetPremiereLettreMajuscule(userDetails.Firstname);
                    Session["group"] = DatabaseGroupTools.GetDefaultIdGroupForUser(userDetails.Id);
                    Session["groupname"] = Utilities.GetPremiereLettreMajuscule(DatabaseGroupTools.GetGroupById((int)Session["group"]).Name);
                    return RedirectToAction("GroupsList", "Groups");
                }
            }
        }

        public ActionResult LogOut()
        {
            int Id = (int)Session["Id"];
            Session.Abandon();
            return RedirectToAction("Index", "Connexion");
        }
    }
}
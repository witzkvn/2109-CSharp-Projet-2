using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.DAL;

namespace WildPay.Controllers
{
    public class ConnexionController : Controller
    {
        // GET: Connexion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(WildPay.Models.User userModel)
        {
            using (WildPayContext db = new WildPayContext())
            {
                var userDetails = db.Users.Where(x => x.Email == userModel.Email && x.Password == userModel.Password).FirstOrDefault();
                if(userDetails==null)
                {
                    userModel.loginErrorMessage = "Email ou mot de passe incorrects.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["Id"] = userDetails.Id;
                    Session["Firstname"] = userDetails.Firstname;
                    return RedirectToAction("Contact", "Home");
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
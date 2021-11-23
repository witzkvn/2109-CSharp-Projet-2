using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.DAL;
using WildPay.Models;
using WildPay.Tools;
using WildPay.BDD;

namespace WildPay.Controllers
{
    public class AccountCreationController : Controller
    {
        public AccountCreationController()
        {
            ViewBag.Message ="First Action View";
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
                if (FormatTools.IsPasswordFormatOk(newUser.Password))
                {
                    newUser.Password = FormatTools.HashPassword(newUser.Password);
                    using (Entities db = new Entities())
                    {
                        db.sp_CreerUser(newUser.Firstname, newUser.Lastname, newUser.Email, newUser.Password);
                    }
                    // ajout de l'utilisateur à faire 
                    // retour à la page d'authentification
                    //return RedirectToAction("Index", "Home");
                    return View("../Home/Index");
                }
                else
                {
                    ViewBag.Message = "Format mot de passe incorrect ";
                }
            }
            return View("Index");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.Models;

namespace WildPay.Controllers
{
    public class AccountCreationController : Controller
    {
        // GET: AccountCreation
        public ActionResult Index(User newUser)
        {
            if (ModelState.IsValid)
            {
                DAL.WildPayContext context = new DAL.WildPayContext();
                context.Users.Add(newUser);
                // Validate User
                // retour à la page d'authentification
                //return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //public ActionResult CreateUser(User newUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Validate User
        //        // retour à la page d'authentification
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
        //}

    }
}
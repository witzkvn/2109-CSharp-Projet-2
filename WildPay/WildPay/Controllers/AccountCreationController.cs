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
                using (Entities db = new Entities())
                {
                    if (db.sp_GetUserByEmail(newUser.Email).SingleOrDefault() != null)
                    {
                        ViewBag.Message = "Email déjà enregistré ! ";
                        return View("Index");

                    }

                    if (FormatTools.IsPasswordFormatOk(newUser.Password))
                    {
                        newUser.Password = FormatTools.HashPassword(newUser.Password);
                        db.sp_CreerUser(newUser.Firstname, newUser.Lastname, newUser.Email, newUser.Password);
                        
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
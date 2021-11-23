using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WildPay.BDD;
using WildPay.DAL;
using WildPay.Models;
using WildPay.Tools;

namespace WildPay.Controllers
{
    public class AccountController : Controller
    {
        private WildPayContext db = new WildPayContext();

        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            User user = db.Users.Find(Session["Id"]);

            var base64 = Convert.ToBase64String(user.UserImage);
            user.UserImageFile = String.Format("data:image/gif;base64,{0}", base64);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id, Firstname, Lastname")] User user)
        {
            if (ModelState.IsValidField("Firstname") && ModelState.IsValidField("Lastname"))
            {
                using (Entities db = new Entities())
                {
                    db.sp_UpdateUser(Session["Id"].ToString(), user.Firstname, user.Lastname);
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //public ActionResult Test()
        //{
        //    return View("Contact", "Home");
        //}
    }
}

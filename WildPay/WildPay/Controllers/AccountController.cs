using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            // userID needs to be stored in Session["userID"] on login
            //if (Session["userID"] == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //User user = db.Users.Find(Session["userID"]);
            User user = db.Users.Find(1);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.UserImageFile = ConverterTools.ByteArrayToImage(user.UserImage);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id,Email,Firstname,Lastname,Password,UserImage")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            User user = db.Users.Find(Session["Id"]);

            if(user.UserImage != null)
            {
                user.UserImageFile = ConverterTools.ByteArrayToStringImage(user.UserImage);
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id, Firstname, Lastname, NewUserImageFile")] User user)
        {
            using (WildPayContext db = new WildPayContext())
            {
                if (ModelState.IsValidField("Firstname") && ModelState.IsValidField("Lastname"))
                {
                    db.Database.ExecuteSqlCommand("sp_UpdateUser @UserId, @firstname, @lastname",
                        new SqlParameter("@UserId", Session["Id"]),
                        new SqlParameter("@firstname", user.Firstname),
                        new SqlParameter("@lastname", user.Lastname)
                    );
                }

                if (user.NewUserImageFile != null)
                {
                    byte[] data = ConverterTools.FileToByteArray(user.NewUserImageFile);

                    db.Database.ExecuteSqlCommand("sp_UpdateUserImageById @UserId, @ImageFile",
                        new SqlParameter("@UserId", Session["Id"]),
                        new SqlParameter("@ImageFile", data)
                    );

                    //user.UserImageFile = ConverterTools.ByteArrayToStringImage(data);
                }
            }                
            return RedirectToAction("Index", "Account");
        }
    }
}

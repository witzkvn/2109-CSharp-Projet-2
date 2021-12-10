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

        public ActionResult Index(string error = null)
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

            ViewBag.ImageUploadMessage = error;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Id, Firstname, Lastname, NewUserImageFile")] User user)
        {
            string updateMessage = "";

            if (!FormatTools.IsTextOk(user.Firstname) || !FormatTools.IsTextOk(user.Lastname))
            {
                updateMessage = "Le format du nom ou du prénom n'est pas correct.";
                return RedirectToAction("Index", "Account", new { error = updateMessage });
            }

            using (WildPayContext db = new WildPayContext())
            {
                if (ModelState.IsValidField("Firstname") && ModelState.IsValidField("Lastname"))
                {
                    db.Database.ExecuteSqlCommand("sp_UpdateUser @UserId, @firstname, @lastname",
                        new SqlParameter("@UserId", Session["Id"]),
                        new SqlParameter("@firstname", user.Firstname),
                        new SqlParameter("@lastname", user.Lastname)
                    );
                } else
                {
                    updateMessage += "Le nom ou le prénom ne sont pas valides\n";
                }

                if (user.NewUserImageFile != null && FormatTools.VerifyImageFormatAndSize(user.NewUserImageFile))
                {
                    byte[] data = ConverterTools.FileToByteArray(user.NewUserImageFile);
                    
                    db.Database.ExecuteSqlCommand("sp_UpdateUserImageById @UserId, @ImageFile",
                        new SqlParameter("@UserId", Session["Id"]),
                        new SqlParameter("@ImageFile", data)
                    );
                } else if(user.NewUserImageFile != null)
                {
                    updateMessage += "Une erreur s'est produite. L'image doit être au format jpg, jpeg ou png, et ne pas dépasser 2 Mo.";
                }
            }                
            return RedirectToAction("Index", "Account", new { error = updateMessage });
        }

        public ActionResult Error()
        {
            return RedirectToAction("Index", "Account", new { error = "Fichier trop volumineux. Le fichier ne doit pas dépasser 2 Mo." });
        }
    }
}

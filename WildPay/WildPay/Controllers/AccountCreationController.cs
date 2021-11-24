﻿using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User newUser)
        {
            if (ModelState.IsValid)
            {
                using (WildPayContext db = new WildPayContext())
                {
                    bool userExists = db.Database.SqlQuery<User>("sp_GetUserByEmail @p0", newUser.Email).Any();

                    if (userExists)
                    {
                        ViewBag.Message = "Email déjà enregistré ! ";
                        return View("Index");
                    }

                    if (FormatTools.IsPasswordFormatOk(newUser.Password))
                    {
                        newUser.Password = FormatTools.HashPassword(newUser.Password);
                        db.Database.ExecuteSqlCommand("sp_CreerUser @firstname, @lastname, @email, @password",
                            new SqlParameter("@firstname", newUser.Firstname),
                            new SqlParameter("@lastname", newUser.Lastname),
                            new SqlParameter("@email", newUser.Email),
                            new SqlParameter("@password", newUser.Password));
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
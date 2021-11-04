using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WildPay.Controllers
{
    public class AccountCreationController : Controller
    {
        // GET: AccountCreation
        public ActionResult Index()
        {
            return View();
        }
        public void OnPost()
        {
            Console.WriteLine("hello ?");
            Console.ReadLine();
        }

    }
}
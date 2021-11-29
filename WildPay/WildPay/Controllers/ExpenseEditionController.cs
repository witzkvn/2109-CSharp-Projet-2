using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WildPay.Models;

namespace WildPay.Controllers
{
    public class ExpenseEditionController : Controller
    {
        // GET: ExpenseEdition
        public ActionResult Index()
        {
            Expense newExpense = new Expense();
            newExpense.CreatedAt = DateTime.Now;
            newExpense.Title = "nouveau titre";
            return View(newExpense);
        }

        public ActionResult EditExpense()
        {
            return View();
        }
    }
}
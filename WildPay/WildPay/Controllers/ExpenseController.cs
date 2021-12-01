using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WildPay.DAL;
using WildPay.Models;
using WildPay.Tools;

namespace WildPay.Controllers
{
    public class ExpenseController : Controller
    {
        WildPayContext db = new WildPayContext();
        // GET: Expense
        public ActionResult Index()
        {
            int GroupId = Utilities.GetGroupePrincipalId();

            List<ExpenseCategoryJoin> expenses = db.Database.SqlQuery<ExpenseCategoryJoin>
                ("sp_GetExpense @p0", GroupId)
                .ToList();
            foreach (ExpenseCategoryJoin exp in expenses)
            {
                exp.DateCourte = FormatTools.ConvertInShortDate(exp.CreatedAt);
                if (exp.UserImage != null)
                {
                    exp.UserImageFile = ConverterTools.ByteArrayToStringImage(exp.UserImage);
                }
            }
            ViewBag.listExpenses = expenses;

            ViewBag.expLabels = new List<string>() { "Clea", "Seb", "Kevin" };
            ViewBag.expValues = new List<double>() { 25.5, 58.2, 16.3 };

            return View();
        }
    }
}
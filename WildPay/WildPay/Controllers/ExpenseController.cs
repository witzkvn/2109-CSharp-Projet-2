using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            var returnCode = new SqlParameter();
            returnCode.ParameterName = "@GroupID ";
            returnCode.SqlDbType = SqlDbType.Int;
            returnCode.Direction = ParameterDirection.Output;

            db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

            int GroupId = (int)returnCode.Value;

            List<ExpenseCategoryJoin> expenses = db.Database.SqlQuery<ExpenseCategoryJoin>
                ("sp_GetExpense @p0", GroupId)
                .ToList();
            foreach (ExpenseCategoryJoin exp in expenses)
            {
                exp.DateCourte = FormatTools.ConvertInShortDate(exp.CreatedAt);
            }
            ViewBag.listExpenses = expenses;

            return View();
        }
    }
}
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
    public class ExpenseEditionController : Controller
    {
        // GET: ExpenseEdition
        public ActionResult Index()
        {
            Expense newExpense = new Expense();
            newExpense.CreatedAt = DateTime.Now;
            ViewBag.listeCategories = CategoryTools.GetCategoriesForDefaultGroup();


            return View(newExpense);
        }

        [HttpPost]
        public ActionResult Validate(Expense newExpense, int categorieId)
        {
            if (ModelState.IsValid)
            {
                int groupId = Utilities.GetGroupePrincipalId();
                using (WildPayContext db = new WildPayContext())
                {


                        db.Database.ExecuteSqlCommand("sp_CreerExpense @date, @title, @value, @user_Id, @category_Id, @GroupId",
                            new SqlParameter("@date", newExpense.CreatedAt),
                            new SqlParameter("@title", newExpense.Title.Trim()),
                            new SqlParameter("@value", newExpense.Value),
                            new SqlParameter("@user_Id", Session["Id"]),
                            new SqlParameter("@category_Id", categorieId),
                            new SqlParameter("@GroupId", groupId));

                        //return RedirectToAction("Index", "Expense");

                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult EditExpense(Expense expenseToEdit)
        {
            return View();
        }
    }
}
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
            ViewBag.title = "Ajouter une dépense";
            ViewBag.listeCategories = CategoryTools.GetCategoriesForDefaultGroup();
            ViewBag.listeUsers = Utilities.GetUsersForGroup();
            return View(newExpense);
        }

        [HttpPost]
        public ActionResult Validate(Expense newExpense, int? categorieId, int auteurId)
        {
            if (ModelState.IsValid)
            {

                CreateOrUpdateExpense(newExpense, categorieId, auteurId);
                return RedirectToAction("Index", "Expense");
            }
            return RedirectToAction("Index");
        }

        private void CreateOrUpdateExpense(Expense newExpense, int? categorieId, int auteurId)
        {
            int groupId = Utilities.GetGroupePrincipalId();
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter dateSql = new SqlParameter("@date", newExpense.CreatedAt);
                SqlParameter titleSql = new SqlParameter("@title", newExpense.Title.Trim());
                SqlParameter valueSql = new SqlParameter("@value", newExpense.Value);
                SqlParameter userSql = new SqlParameter("@user_Id", auteurId);
                SqlParameter categorySql = new SqlParameter("@category_Id", SqlDbType.Int);
                SqlParameter expenseIdSql = new SqlParameter("@expense_Id", newExpense.Id);
                categorySql.Value = (object)categorieId ?? DBNull.Value;

                bool expenseExists = db.Database.SqlQuery<Expense>("sp_GetExpenseById @p0", newExpense.Id).Any();
                if (expenseExists)
                {
                    db.Database.ExecuteSqlCommand("sp_UpdateExpense @expense_Id, @date, @title, @value, @user_Id, @category_Id",
                        expenseIdSql, dateSql, titleSql, valueSql, userSql, categorySql);
                }
                else
                {
                    db.Database.ExecuteSqlCommand("sp_CreerExpense @date, @title, @value, @user_Id, @category_Id, @GroupId",
                        dateSql, titleSql, valueSql, userSql, categorySql, new SqlParameter("@GroupId", groupId));
                }
            }
        }

        public ActionResult EditExpense(int idExpense)
        {
            Expense expenseToEdit = Utilities.GetExpenseById(idExpense);
            ViewBag.title = "Editer une dépense";
            ViewBag.listeCategories = CategoryTools.GetCategoriesForDefaultGroup();
            ViewBag.listeUsers = Utilities.GetUsersForGroup();
            return View("Index", expenseToEdit);
        }


    }
}
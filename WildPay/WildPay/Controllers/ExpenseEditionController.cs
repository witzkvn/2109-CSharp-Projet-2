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
        public ActionResult Index()
        {
            Expense newExpense = new Expense();
            if (Session["Id"] != null)
                newExpense.FkUserId = (int)Session["Id"];
            ViewBag.title = "Ajouter une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            ViewBag.listeUsers = DatabaseTools.GetUsersForGroup();
            ViewBag.date = DateTime.Now.ToString("yyyy-MM-dd");
            return View(newExpense);
        }

        [HttpPost]
        public ActionResult Validate(Expense newExpense, int? categorieId, int auteurId, DateTime newDate)
        {
            newExpense.FkCategoryId = categorieId;
            newExpense.FkUserId = auteurId;
            newExpense.CreatedAt = newDate;
            if (ModelState.IsValid)
            {
                DatabaseTools.CreateOrUpdateExpense(newExpense);
                string confirmation = newExpense.Id != 0 ? "La dépense a bien été éditée" : "La dépense a bien été ajoutée";
                return RedirectToAction("Index", "Expense", new { confirmationMessage = confirmation });
            }
            else
            {
                ViewBag.title = "Ajouter une dépense";
                ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
                ViewBag.listeUsers = DatabaseTools.GetUsersForGroup();
                ViewBag.date = newDate.ToString("yyyy-MM-dd");
                return View("Index", newExpense);
            }
        }



        public ActionResult EditExpense(int expenseId)
        {
            Expense expenseToEdit = DatabaseTools.GetExpenseById(expenseId);
            ViewBag.title = "Editer une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            ViewBag.listeUsers = DatabaseTools.GetUsersForGroup();
            ViewBag.date = expenseToEdit.CreatedAt.ToString("yyyy-MM-dd");
            return View("Index", expenseToEdit);
        }

        public ActionResult DeleteExpense(int expenseId)
        {
            Expense expenseToEdit = DatabaseTools.GetExpenseById(expenseId);
            ViewBag.title = "Editer une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            ViewBag.listeUsers = DatabaseTools.GetUsersForGroup();
            ViewBag.date = expenseToEdit.CreatedAt.ToString("yyyy-MM-dd");
            ViewBag.expenseToDelete = expenseToEdit;
            return View("Index", expenseToEdit);
        }

        public ActionResult ConfirmDeleteExpense(int idExpense)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter expenseSql = new SqlParameter("@expense_Id", idExpense);
                db.Database.ExecuteSqlCommand("sp_DeleteExpense @expense_Id", expenseSql);
            }
            return RedirectToAction("Index", "Expense");
        }



    }
}
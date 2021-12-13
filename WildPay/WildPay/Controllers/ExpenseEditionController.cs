using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            Expense newExpense = new Expense();
            if (Session["Id"] != null)
                newExpense.FkUserId = (int)Session["Id"];
            ViewBag.title = "Ajouter une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup((int)Session["group"]);
            ViewBag.date = DateTime.Now.ToString("yyyy-MM-dd");
            return View(newExpense);
        }

        [HttpPost]
        public ActionResult Validate(Expense newExpense, int? categorieId, int auteurId, DateTime newDate)
        {
            if (!FormatTools.IsTextAndNumberOk(newExpense.Title))
            {
                ViewBag.Error = "Le titre de la dépense ne doit contenir que des lettres ou des chiffres.";
                ViewBag.title = newExpense.Id == 0 ? "Ajouter une dépense" : "Editer une dépense";
                ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
                ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup((int)Session["group"]);
                ViewBag.date = newDate.ToString("yyyy-MM-dd");
                return View("Index", newExpense);
            }

            newExpense.FkCategoryId = categorieId;
            newExpense.FkUserId = auteurId;
            newExpense.CreatedAt = newDate;
            if (ModelState.IsValid)
            {
                DatabaseTools.CreateOrUpdateExpense(newExpense, (int)Session["group"]);
                string confirmation = newExpense.Id != 0 ? "La dépense a bien été éditée" : "La dépense a bien été ajoutée";
                return RedirectToAction("Index", "Expense", new { confirmationMessage = confirmation });
            }
            else
            {
                ViewBag.title = newExpense.Id == 0 ? "Ajouter une dépense" : "Editer une dépense";
                ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
                ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup((int)Session["group"]);
                ViewBag.date = newDate.ToString("yyyy-MM-dd");
                return View("Index", newExpense);
            }
        }



        public ActionResult EditExpense(int expenseId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            Expense expenseToEdit = DatabaseTools.GetExpenseById(expenseId);
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], expenseToEdit.FkGroupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (expenseToEdit == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.title = "Editer une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup((int)Session["group"]);
            ViewBag.date = expenseToEdit.CreatedAt.ToString("yyyy-MM-dd");
            return View("Index", expenseToEdit);
        }

        public ActionResult DeleteExpense(int expenseId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            Expense expenseToEdit = DatabaseTools.GetExpenseById(expenseId);
            if(expenseToEdit == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.title = "Editer une dépense";
            ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup((int)Session["group"]);
            ViewBag.date = expenseToEdit.CreatedAt.ToString("yyyy-MM-dd");
            ViewBag.expenseToDelete = expenseToEdit;
            return View("Index", expenseToEdit);
        }

        public ActionResult ConfirmDeleteExpense(int idExpense)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter expenseSql = new SqlParameter("@expense_Id", idExpense);
                db.Database.ExecuteSqlCommand("sp_DeleteExpense @expense_Id", expenseSql);
            }
            return RedirectToAction("Index", "Expense");
        }



    }
}
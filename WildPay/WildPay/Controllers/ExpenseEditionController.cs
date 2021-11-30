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
            ViewBag.listeUsers = GetUsersForGroup(Utilities.GetGroupePrincipalId());


            return View(newExpense);
        }

        [HttpPost]
        public ActionResult Validate(Expense newExpense, int? categorieId, int auteurId)
        {
            if (ModelState.IsValid)
            {
                int groupId = Utilities.GetGroupePrincipalId();
                using (WildPayContext db = new WildPayContext())
                {
                    SqlParameter categoryId = new SqlParameter ("@category_Id", SqlDbType.Int);
                    categoryId.Value = (object)categorieId ?? DBNull.Value;

                    db.Database.ExecuteSqlCommand("sp_CreerExpense @date, @title, @value, @user_Id, @category_Id, @GroupId",
                        new SqlParameter("@date", newExpense.CreatedAt),
                        new SqlParameter("@title", newExpense.Title.Trim()),
                        new SqlParameter("@value", newExpense.Value),
                        new SqlParameter("@user_Id", auteurId),
                        categoryId,
                        new SqlParameter("@GroupId", groupId));

                    return RedirectToAction("Index", "Expense");

                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult EditExpense(Expense expenseToEdit)
        {
            return View();
        }


        public List<User> GetUsersForGroup(int idGroup)
        {
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                var type = returnCode.Value.GetType().FullName;

                if (type != "System.DBNull")
                {
                    int groupId = (int)returnCode.Value;
                    List<User> users = db.Database.SqlQuery<User>
                    ("sp_GetUsersForGroup @p0", groupId)
                    .ToList();
                    return users;
                }
                return null;
            }
        }
    }
}
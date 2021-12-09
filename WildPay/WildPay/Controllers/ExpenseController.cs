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
        int GroupId = Utilities.GetGroupePrincipalId();
        // GET: Expense
        public ActionResult Index(string confirmationMessage = null)
        {
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

            List<ListUser> user = db.Database.SqlQuery<ListUser>
                    ("sp_GetUser @p0", GroupId)
                    .ToList();

            ViewBag.listeUsers = user;
            ViewBag.SommesDues = SommesDues(ViewBag.listeUsers);
            ViewBag.userID = Convert.ToInt32(Session["Id"]);

            Dictionary<string, double> nameSumByUser = new Dictionary<string, double>();
            nameSumByUser = db.Database.SqlQuery<SommeParUser>
                    ("sp_SommeParUser @p0", GroupId)
                    .ToDictionary(k => k.PrenomNom, v => v.Somme);

            List<string> prenomNom = new List<string>();
            prenomNom = nameSumByUser.Select(kvp => kvp.Key).ToList();

            List<double> somme = new List<double>();
            somme = nameSumByUser.Select(kvp => kvp.Value).ToList();

            ViewBag.expLabels = prenomNom;
            ViewBag.expValues = somme;

            Dictionary<string, double> nameSumByCat = new Dictionary<string, double>();
            nameSumByCat = db.Database.SqlQuery<SommeParCategory>
                    ("sp_SommeParCategory @p0", GroupId)
                    .ToDictionary(k => k.NameCategory, v => v.SommeCategory);

            double SumCatNull = db.Expenses.Where(e => e.FkCategoryId == null).Sum(e => e.Value);

            nameSumByCat.Add("Autre", SumCatNull);

            List<string> nameCat = new List<string>();
            nameCat = nameSumByCat.Select(kvp => kvp.Key).ToList();

            List<double> sommeCat = new List<double>();
            sommeCat = nameSumByCat.Select(kvp => kvp.Value).ToList();

            ViewBag.expLabels2 = nameCat;
            ViewBag.expValues2 = sommeCat;

            ViewBag.Confirm = confirmationMessage;
            return View();
        }

        private Dictionary<int, double> SommesDues(List<ListUser> listeUser)
        {
            Dictionary<int, double> sommesDues = new Dictionary<int, double>();
            
            foreach(ListUser user in listeUser)
            {
                double sommeDueParUser = 0;
                double sommeDueParUserShort = 0;
                var returnSomme = new SqlParameter();
                returnSomme.ParameterName = "@sommeDue ";
                returnSomme.SqlDbType = SqlDbType.Money;
                returnSomme.Direction = ParameterDirection.Output;

                var returnSomme2 = new SqlParameter();
                returnSomme2.ParameterName = "@sommeDue ";
                returnSomme2.SqlDbType = SqlDbType.Money;
                returnSomme2.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetSommeDue @UserId, @groupId, @sommeDue OUTPUT",
                    new SqlParameter("@UserId", user.Id),
                    new SqlParameter("@groupId", GroupId),
                    returnSomme);

                string returnSommeToString = returnSomme.Value.ToString();

                if(returnSommeToString != "")
                {
                    sommeDueParUser = Convert.ToDouble(returnSommeToString);
                    sommeDueParUserShort = FormatTools.ConvertinShortDouble(sommeDueParUser);
                }
                else
                {
                    db.Database.ExecuteSqlCommand("sp_GetSommeDueSiNull @groupId, @sommeDue OUTPUT",
                    new SqlParameter("@groupId", GroupId),
                    returnSomme2);
                    string returnSommeToStringSiNull = returnSomme2.Value.ToString();
                    if (returnSommeToStringSiNull == "") returnSommeToStringSiNull = "0";
                    sommeDueParUser = Convert.ToDouble(returnSommeToStringSiNull);
                    sommeDueParUserShort = FormatTools.ConvertinShortDouble(sommeDueParUser);
                }

                sommesDues.Add(user.Id, sommeDueParUserShort);
            }

            return sommesDues;
        }
    }
}
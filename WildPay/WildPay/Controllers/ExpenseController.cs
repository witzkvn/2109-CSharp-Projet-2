﻿using System;
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
        int GroupId = Utilities.GetGroupePrincipalId();
        // GET: Expense
        public ActionResult Index()
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
                    returnSomme);
                    string returnSommeToStringSiNull = returnSomme.Value.ToString();
                    sommeDueParUser = Convert.ToDouble(returnSommeToStringSiNull);
                    sommeDueParUserShort = FormatTools.ConvertinShortDouble(sommeDueParUser);
                }

                sommesDues.Add(user.Id, sommeDueParUserShort);
            }

            return sommesDues;
        }

    }
}
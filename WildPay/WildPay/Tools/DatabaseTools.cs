using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using WildPay.DAL;
using WildPay.Models;
using System.Data.Entity;

namespace WildPay.Tools
{
    public class DatabaseTools
    {
        public static List<Category> GetCategoriesForDefaultGroup()
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
                    List<Category> categories = db.Database.SqlQuery<Category>
                    ("sp_GetCategory @p0", groupId).OrderBy(cat => cat.Name)
                    .ToList();
                    return categories;
                }
                return null;
            }
        }

        public static Expense GetExpenseById(int idExpense)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter idExpenseSQL = new SqlParameter("@expenseId", idExpense);
                return db.Database.SqlQuery<Expense>("sp_GetExpenseById @expenseId", idExpenseSQL).FirstOrDefault();
            }
        }


        public static List<User> GetUsersForGroup()
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
                    ("sp_GetUsersForGroup @p0", groupId).OrderBy(user => user.Firstname)
                    .ToList();
                    return users;
                }
                return null;
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (WildPayContext db = new WildPayContext())
            {
                users = db.Users.ToList(); // eventuellement changer en SP 
            }
            return users;
        }


        public static void CreateOrUpdateExpense(Expense newExpense)
        {
            int groupId = Utilities.GetGroupePrincipalId();
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter dateSql = new SqlParameter("@date", newExpense.CreatedAt);
                SqlParameter titleSql = new SqlParameter("@title", newExpense.Title.Trim());
                SqlParameter valueSql = new SqlParameter("@value", newExpense.Value);
                SqlParameter userSql = new SqlParameter("@user_Id", newExpense.FkUserId);
                SqlParameter categorySql = new SqlParameter("@category_Id", SqlDbType.Int);
                SqlParameter expenseIdSql = new SqlParameter("@expense_Id", newExpense.Id);
                categorySql.Value = (object)newExpense.FkCategoryId ?? DBNull.Value;

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
    }
}
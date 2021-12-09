using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WildPay.DAL;
using WildPay.Models;

namespace WildPay.Tools
{
    public class DatabaseGroupTools
    {
        public static List<Group> GetGroupsForUser(int userID)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter userSql = new SqlParameter("@user_Id", userID);
                List<Group> groups = db.Database.SqlQuery<Group>
                ("sp_GetGroupsForUser @user_Id", userSql).OrderBy(group => group.Name)
                .ToList();
                return groups;
            }
        }

        public static int CreateGroupForUser(int userID, string groupName)
        {
            int GroupId = 0;
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;
                SqlParameter userSql = new SqlParameter("@user_Id", userID);
                SqlParameter nameSql = new SqlParameter("@name", groupName);
                db.Database.ExecuteSqlCommand("sp_CreerGroup @name, @user_Id, @GroupID OUTPUT", nameSql, userSql, returnCode);
                GroupId = (int)returnCode.Value;
                return GroupId;
            }
        }

        internal static void AddMemberToGroup(int memberId, int id)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter memberIdSql = new SqlParameter("@user_Id", memberId);
                SqlParameter groupSql = new SqlParameter("@group_Id", id);

                db.Database.ExecuteSqlCommand("sp_AddMemberToGroup @user_Id, @group_Id",
                    memberIdSql, groupSql);
            }
        }

    }
}
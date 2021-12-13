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

        internal static void AddBaseCategories(int groupId)
        {
            using (WildPayContext db = new WildPayContext())
            {
                db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                        new SqlParameter("@name", "restaurant"),
                        new SqlParameter("@group_Id", groupId),
                        new SqlParameter("@IsBase", true));
                db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                        new SqlParameter("@name", "transport"),
                        new SqlParameter("@group_Id", groupId),
                        new SqlParameter("@IsBase", true));
                db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                        new SqlParameter("@name", "courses"),
                        new SqlParameter("@group_Id", groupId),
                        new SqlParameter("@IsBase", true));
                db.Database.ExecuteSqlCommand("sp_CreerCategory @name, @group_Id, @IsBase",
                        new SqlParameter("@name", "hebergement"),
                        new SqlParameter("@group_Id", groupId),
                        new SqlParameter("@IsBase", true));
            }
        }

        public static bool AddedMemberToGroup(string memberMail, int groupId)
        {
            bool isOk;
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter mailSql = new SqlParameter("@UserEmail", memberMail);
                SqlParameter groupSql = new SqlParameter("@group_Id", groupId);
                var returnCode = new SqlParameter("@ajoutOk", 0);
                returnCode.SqlDbType = SqlDbType.Bit;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_AddMemberToGroup @UserEmail, @group_Id, @ajoutOk OUTPUT", mailSql, groupSql, returnCode);
                isOk = (bool)returnCode.Value;
                return isOk;
            }
        }

        public static List<User> GetUsersForGroup(int groupId)
        {
            using (WildPayContext db = new WildPayContext())
            {
                List<User> users = db.Database.SqlQuery<User>
                ("sp_GetUsersForGroup @p0", groupId).OrderBy(user => user.Firstname)
                .ToList();
                return users;
            }
        }

        public static int GetDefaultIdGroupForUser(int userId)
        {
            int GroupId = 0;
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;
                SqlParameter userSql = new SqlParameter("@user_Id", userId);
                db.Database.ExecuteSqlCommand("sp_GetDefaultGroupForUser @user_Id, @GroupID OUTPUT", userSql, returnCode);
                GroupId = (int)returnCode.Value;
                return GroupId;
            }
        }

        public static Group GetGroupById(int id)
        {
            using (WildPayContext db = new WildPayContext())
            {
                Group group = db.Database.SqlQuery<Group>
                ("sp_GetGroupById @p0", id).FirstOrDefault();
                return group;
            }
        }

    }
}
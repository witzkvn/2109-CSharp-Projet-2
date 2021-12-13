using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class GroupsController : Controller
    {

        public ActionResult GroupsList(string errorMessage = null, string validationMessage=null)
        {
            if (errorMessage != null)
            {
                ViewBag.Error = errorMessage;
            }
            if (validationMessage != null)
            {
                ViewBag.Confirm = validationMessage;
            }
            List<Group> groupes =  DatabaseGroupTools.GetGroupsForUser((int)Session["Id"]);
            return View(groupes);
        }


        public ActionResult GroupCreation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupCreation(Group group)
        {
            if (ModelState.IsValid)
            {
                Session["group"] = DatabaseGroupTools.CreateGroupForUser((int)Session["id"], group.Name);
                Session["groupname"] = DatabaseGroupTools.GetGroupById((int)Session["group"]).Name;
                DatabaseGroupTools.AddBaseCategories((int)Session["group"]);
                return RedirectToAction("GroupsList", new { validationMessage = "Le groupe a bien été créé" });
            }

            return View(group);
        }


        public ActionResult GroupEdit(int groupId)
        {
            if (groupId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = DatabaseGroupTools.GetGroupById(groupId);
            SetIdAndNameForGroup(groupId);
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit(Group group, string memberMail)
        {
            if (ModelState.IsValid)
            {
                if (memberMail != "")
                {
                    bool memberAdded = DatabaseGroupTools.AddedMemberToGroup(memberMail, group.Id);
                }
                else
                {
                    // update name
                }

                return RedirectToAction("GroupsList", new { errorMessage = "", validationMessage = "le groupe a bien été modifié" });
            }
            ViewBag.title = "Edition du groupe";
            return View(group);
        }

        public ActionResult DeleteMember(int memberToDeleteId, int groupId)
        {
            using (WildPayContext db = new WildPayContext())
            {
                User user = db.Users.Where(u => u.Id == memberToDeleteId).First();
                ViewBag.user = user;
            }

            Group group = DatabaseGroupTools.GetGroupById(groupId);
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View("GroupEdit", group);
        }

        public ActionResult ConfirmDeleteMember(int userId, int groupId)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter userSql = new SqlParameter("@User_Id", userId);
                SqlParameter groupSql = new SqlParameter("@groupId", groupId);
                db.Database.ExecuteSqlCommand("sp_DeleteMemberGroup @User_Id, @groupId", userSql, groupSql);
            }

            return RedirectToAction("GroupEdit", new {groupId = groupId});
        }



        public ActionResult DeleteGroup(int groupId)
        {
            if (groupId == Utilities.GetGroupePrincipalId()) 
            {
                SetIdAndNameForGroup(groupId);
                return RedirectToAction("GroupsList",  new {errorMessage = "Le groupe principal ne peut pas être supprimé."});
            } 
            Group groupToEdit = DatabaseGroupTools.GetGroupById(groupId);
            ViewBag.groupToDelete = groupToEdit;
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(groupId);
            return View("GroupEdit", groupToEdit);
        }

        public ActionResult ConfirmDeleteGroup(int groupId)
        {
            if (groupId == Utilities.GetGroupePrincipalId()) {
                SetIdAndNameForGroup(groupId);
                return RedirectToAction("GroupsList", new { errorMessage = "Le groupe principal ne peut pas être supprimé." });
            } 
            using (WildPayContext db = new WildPayContext())
            {
                Group groupToDelete = db.Groups.Where(g => g.Id == groupId).First();
                List<Category> cat = db.Categories.Include(c => c.Groups).Where(c => c.Groups.Any(g => g.Id == groupId)).ToList();
                db.Groups.Remove(groupToDelete);
                db.SaveChanges();
                db.Categories.RemoveRange(cat);
                db.SaveChanges();
            }

            if (groupId == (int)Session["group"])
            {
                SetDefaultGroupForUser();
            }
            return RedirectToAction("GroupsList", "Groups", new { errorMessage = "", validationMessage = "Le groupe a bien été supprimé" });
        }

        public ActionResult ChangeGroup(int groupId)
        {
            SetIdAndNameForGroup(groupId);
            return RedirectToAction("GroupsList");
        }

        private void SetIdAndNameForGroup(int groupId)
        {
            Session["group"] = groupId;
            Session["groupname"] = DatabaseGroupTools.GetGroupById(groupId).Name;
        }

        private void SetDefaultGroupForUser()
        {
            Session["group"] = DatabaseGroupTools.GetDefaultIdGroupForUser((int)Session["id"]);
            Session["groupname"] = DatabaseGroupTools.GetGroupById((int)Session["group"]).Name;
        }

    }
}

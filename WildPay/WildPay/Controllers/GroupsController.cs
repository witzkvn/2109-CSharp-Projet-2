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
                ViewBag.Validation = validationMessage;
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
        public ActionResult GroupCreation([Bind(Include = "Id,CreatedAt,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                Session["group"] = DatabaseGroupTools.CreateGroupForUser((int)Session["id"], group.Name);
                Session["groupname"] = DatabaseGroupTools.GetGroupById((int)Session["group"]).Name;
                DatabaseGroupTools.AddBaseCategories((int)Session["group"]);
                List<Group> groupes = DatabaseGroupTools.GetGroupsForUser((int)Session["Id"]);
                return View("GroupsList", groupes);
            }

            return View(group);
        }


        public ActionResult GroupEdit(int groupId)
        {
            if (groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = DatabaseGroupTools.GetGroupById(groupId);
            Session["group"] = groupId;
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(groupId);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit([Bind(Include = "Id,CreatedAt,Name")] Group group, string memberMail)
        {
            if (ModelState.IsValid)
            {
                if (memberMail != "")
                {
                    bool ok = DatabaseGroupTools.AddedMemberToGroup(memberMail, group.Id);
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


        public ActionResult DeleteGroup(int groupId)
        {
            Group groupToEdit = DatabaseGroupTools.GetGroupById(groupId);
            ViewBag.groupToDelete = groupToEdit;
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(groupId);
            return View("GroupEdit", groupToEdit);
        }

        public ActionResult ConfirmDeleteGroup(int groupId)
        {
            using (WildPayContext db = new WildPayContext())
            {
                SqlParameter groupSql = new SqlParameter("@group_Id", groupId);
                db.Database.ExecuteSqlCommand("sp_DeleteGroup @group_Id", groupSql);
            }
            if (groupId == (int)Session["group"])
            {
                Session["group"] = DatabaseGroupTools.GetDefaultIdGroupForUser((int)Session["id"]);
                Session["groupname"] = DatabaseGroupTools.GetGroupById((int)Session["group"]).Name;
            }
            return RedirectToAction("GroupsList", "Groups", new { errorMessage = "", validationMessage = "le groupe a bien été supprimé" });
        }

        public ActionResult ChangeGroup(int groupId)
        {
            Group groupSelected = DatabaseGroupTools.GetGroupById(groupId);
            Session["group"] = groupId;
            Session["groupname"] = DatabaseGroupTools.GetGroupById(groupId).Name;
            List<Group> groupes = DatabaseGroupTools.GetGroupsForUser((int)Session["Id"]);
            return View("GroupsList", groupes);
        }

    }
}

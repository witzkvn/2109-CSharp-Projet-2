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
                DatabaseGroupTools.AddBaseCategories((int)Session["group"]);

                return RedirectToAction("GroupsList");
            }

            return View(group);
        }


        public ActionResult GroupEdit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = DatabaseGroupTools.GetGroupById(id);
            Session["group"] = id;
            ViewBag.listeUsers = DatabaseGroupTools.GetUsersForGroup(id);

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
            return View(group);
        }


        public ActionResult DeleteGroup(int groupId)
        {
            Group groupToEdit = DatabaseGroupTools.GetGroupById(groupId);
            ViewBag.title = "Editer un groupe";
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
            }
            return RedirectToAction("GroupsList", "Groups", new { errorMessage = "", validationMessage = "le groupe a bien été supprimé" });
        }

    }
}

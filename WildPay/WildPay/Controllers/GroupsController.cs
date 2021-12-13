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
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            Dictionary<Group, int> groupes =  DatabaseGroupTools.GetGroupsForUser((int)Session["Id"]);

            return View(groupes);
        }


        public ActionResult GroupCreation()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupCreation(Group group)
        {
            if (ModelState.IsValid && FormatTools.IsTextAndNumberOk(group.Name))
            {
                Session["group"] = DatabaseGroupTools.CreateGroupForUser((int)Session["id"], group.Name);
                Session["groupname"] = DatabaseGroupTools.GetGroupById((int)Session["group"]).Name;
                DatabaseGroupTools.AddBaseCategories((int)Session["group"]);
                return RedirectToAction("GroupsList", new { validationMessage = "Le groupe a bien été créé" });
            }
            ViewBag.Error = "Une erreur est survenue. Le groupe n'a pas été créé.";
            return View(group);
        }


        public ActionResult GroupEdit(int groupId, string errorMessage = null, string validationMessage = null)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId))
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
            ViewBag.Error = errorMessage;
            ViewBag.Confirm = validationMessage;
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit(Group group, string memberMail)
        {
            string confirmation = "";
            string error = "";

            if (ModelState.IsValid && FormatTools.IsTextAndNumberOk(group.Name))
            {
                if (memberMail != "")
                {
                    bool memberAdded = DatabaseGroupTools.AddedMemberToGroup(memberMail, group.Id);
                    if (memberAdded == true)
                    {
                        confirmation += "Le membre a bien été ajouté.";
                    }
                    else
                    {
                        error += "Une erreur est survenue. Le membre n'a pas été ajouté.";
                    }
                }

                using (WildPayContext db = new WildPayContext())
                {
                    string dbName = db.Groups.Find(group.Id).Name;
                    if(dbName != group.Name)
                    {
                        db.Groups.Find(group.Id).Name = group.Name;
                        db.SaveChanges();
                        confirmation += "\nLe groupe a bien été mis à jour.";
                        Session["group"] = group.Id;
                        Session["groupname"] = group.Name;
                    } 
                }
                return RedirectToAction("GroupEdit", new { groupId = group.Id, errorMessage = error, validationMessage = confirmation });
            }

            error += "Une erreur est survenue. Le groupe n'a pas été modifié.";
            ViewBag.Error = error;
            ViewBag.Confirm = confirmation;
            return View(group);
        }

        public ActionResult DeleteMember(int memberToDeleteId, int groupId)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId) || 
                memberToDeleteId == 0 || 
                DatabaseGroupTools.IsPrincipalGroup(groupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId) || userId == 0 ||
                DatabaseGroupTools.IsPrincipalGroup(groupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId) || DatabaseGroupTools.IsPrincipalGroup(groupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId) || DatabaseGroupTools.IsPrincipalGroup(groupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (!DatabaseGroupTools.IsPartOfGroup((int)Session["Id"], groupId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

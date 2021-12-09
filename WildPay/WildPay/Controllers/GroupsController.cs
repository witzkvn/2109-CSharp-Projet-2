using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        private WildPayContext db = new WildPayContext();


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
                return RedirectToAction("GroupsList");
            }

            return View(group);
        }


        public ActionResult GroupEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            ViewBag.listeUsers = db.Users.ToList();
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit([Bind(Include = "Id,CreatedAt,Name")] Group group, int memberId)
        {
            if (ModelState.IsValid)
            {
                if (memberId !=0)
                {
                    DatabaseGroupTools.AddMemberToGroup(memberId, group.Id);
                }
                else
                {
                    // update name
                }

                return RedirectToAction("GroupsList", new { errorMessage = "", validationMessage = "le groupe a bien été modifié" });
            }
            return View(group);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

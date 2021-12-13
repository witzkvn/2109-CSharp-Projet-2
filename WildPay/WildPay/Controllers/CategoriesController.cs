using System;
using System.Collections.Generic;
using System.Data;
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
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index(string deleteMessage = null)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (deleteMessage != null)
            {
                ViewBag.Confirm = deleteMessage;
            }
            ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
            return View();
        }

        [HttpPost]
        public ActionResult Index(Category newCategory)
        {
                if (ModelState.IsValid && FormatTools.IsTextAndNumberOk(newCategory.Name))
            {
                using (WildPayContext db = new WildPayContext())
                {
                    if (!CategoryAlreadyExists(db, newCategory))
                    {
                        AddCategory(db, newCategory);
                        ViewBag.Confirm = "Categorie ajoutée ";
                        ViewBag.Error = "";
                    }
                    else
                    {
                        ViewBag.Confirm = "";
                        ViewBag.Error = "La categorie existe déjà ";
                    }
                }
            }
            else {
                ViewBag.Confirm = "";
                ViewBag.Error = "Categorie incorrecte";
            } 

            ViewBag.listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);

            return View("Index");
        }

        public ActionResult DeleteCategorie(int categorieIDToDelete)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (!DatabaseGroupTools.IsUserAllowedToAccessCategory((int)Session["Id"], categorieIDToDelete))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Category> listeCategories = DatabaseTools.GetCategoriesFromGroup((int)Session["group"]);
            Category categoryASupprimer = listeCategories.Where(c => c.Id == categorieIDToDelete).First();
            if (categoryASupprimer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.listeCategories = listeCategories;
            ViewBag.categoryASupprimer = categoryASupprimer;
            return View("Index");
        }

        public ActionResult ConfirmDeleteCategorie(int idCategory)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Index", "Connexion");
            }
            if (!DatabaseGroupTools.IsUserAllowedToAccessCategory((int)Session["Id"], idCategory))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string deleteMsg ="Une erreur est survenue.";
            using (WildPayContext db = new WildPayContext())
            {
                Category selectedCategory = db.Categories.Where(cat => cat.Id == idCategory).FirstOrDefault();
                if (selectedCategory != null && !selectedCategory.IsBase)
                {
                    SqlParameter categorieID = new SqlParameter("@CategoryId", idCategory);
                    db.Database.ExecuteSqlCommand("sp_SuppressionCategory @CategoryId", categorieID);
                    deleteMsg = "Categorie supprimée";
                }
            }
            return RedirectToAction("Index", new { deleteMessage = deleteMsg });
        }

        private bool CategoryAlreadyExists(WildPayContext db, Category newCategory)
        {
            SqlParameter categoryName = new SqlParameter("@name", newCategory.Name);
            int currentGroupId = (int)Session["group"];
            SqlParameter groupId = new SqlParameter("@group_Id", currentGroupId);
            return db.Database.SqlQuery<Category>
                ("sp_GetCategoryByName @name, @group_Id",
                categoryName, groupId)
                .Any();
        }

        private void AddCategory(WildPayContext db, Category newCategory)
        {
            SqlParameter newName = new SqlParameter("@name", newCategory.Name);
            int currentGroupId = (int)Session["group"];
            SqlParameter idGroup = new SqlParameter("@group_Id", currentGroupId);
            db.Database.ExecuteSqlCommand
                ("sp_CreerCategory @name, @group_Id, @IsBase",
                newName, 
                idGroup,
                new SqlParameter("@IsBase", false));
        }

    }
}
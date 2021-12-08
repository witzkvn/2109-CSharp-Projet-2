using System;
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
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index(string deleteMessage = null)
        {
            if (deleteMessage != null)
            {
                ViewBag.Confirm = deleteMessage;
            }
            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            return View();
        }

        [HttpPost]
        public ActionResult Index(Category newCategory)
        {
            if (ModelState.IsValid)
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

            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();

            return View("Index");
        }

        public ActionResult DeleteCategorie(int categorieIDToDelete)
        {
            List<Category> listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            ViewBag.listeCategories = listeCategories;
            ViewBag.categoryASupprimer = listeCategories.Where(c => c.Id == categorieIDToDelete).First();
            return View("Index");
        }

        public ActionResult ConfirmDeleteCategorie(int idCategory)
        {
            using (WildPayContext db = new WildPayContext())
            {
                Category selectedCategory = db.Categories.Where(cat => cat.Id == idCategory).FirstOrDefault();
                if (selectedCategory != null && !selectedCategory.IsBase)
                {
                    SqlParameter categorieID = new SqlParameter("@CategoryId", idCategory);
                    db.Database.ExecuteSqlCommand("sp_SuppressionCategory @CategoryId", categorieID);
                }
            }
            ViewBag.listeCategories = DatabaseTools.GetCategoriesForDefaultGroup();
            //return View("Index");
            return RedirectToAction("Index", new { deleteMessage = "Categorie supprimée " });
        }

        private bool CategoryAlreadyExists(WildPayContext db, Category newCategory)
        {
            SqlParameter categoryName = new SqlParameter("@name", newCategory.Name);
            int principalGroupId = Utilities.GetGroupePrincipalId();
            SqlParameter groupId = new SqlParameter("@group_Id", principalGroupId);
            return db.Database.SqlQuery<Category>
                ("sp_GetCategoryByName @name, @group_Id",
                categoryName, groupId)
                .Any();
        }

        private void AddCategory(WildPayContext db, Category newCategory)
        {
            SqlParameter newName = new SqlParameter("@name", newCategory.Name);
            int principalGroupId = Utilities.GetGroupePrincipalId();
            SqlParameter idGroup = new SqlParameter("@group_Id", principalGroupId);
            db.Database.ExecuteSqlCommand
                ("sp_CreerCategory @name, @group_Id, @IsBase",
                newName, 
                idGroup,
                new SqlParameter("@IsBase", false));
        }

    }
}
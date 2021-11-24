using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            using (WildPayContext db = new WildPayContext())
            {
                List<Category> categories = db.Database.SqlQuery<Category>
                    ("sp_GetCategory @p0", "1")
                    .ToList();
                ViewBag.listeCategories = categories;
            }
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
                        ViewBag.Message = "Categorie ajoutée ";
                    }
                    else
                    {
                        ViewBag.Message = "La categorie existe déjà ";
                    }
                }
            }
            else ViewBag.Message = "Categorie incorrecte";
            return RedirectToAction("Index", "Categories");
        }

        public ActionResult DeleteCategorie(int categorieToDelete)
        {
            using (WildPayContext db = new WildPayContext())
            {
                if (categorieToDelete != null)
                {
                    SqlParameter categorieID = new SqlParameter("@CategoryId", categorieToDelete);
                    db.Database.ExecuteSqlCommand("sp_SuppressionCategory @CategoryId", categorieID);
                }
            }
            return RedirectToAction("Index", "Categories");
        }

        private bool CategoryAlreadyExists(WildPayContext db, Category newCategory)
        {
            SqlParameter categoryName = new SqlParameter("@name", newCategory.Name);
            SqlParameter groupId = new SqlParameter("@group_Id", "1");
            return db.Database.SqlQuery<Category>
                ("sp_GetCategoryByName @name, @group_Id",
                categoryName, groupId)
                .Any();
        }

        private void AddCategory(WildPayContext db, Category newCategory)
        {
            SqlParameter newName = new SqlParameter("@name", newCategory.Name);
            SqlParameter idGroup = new SqlParameter("@group_Id", "1");
            db.Database.ExecuteSqlCommand
                ("sp_CreerCategory @name, @group_Id",
                newName, idGroup);
        }

    }
}
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
        public ActionResult Index()
        {
            using (WildPayContext db = new WildPayContext())
            {
                var returnCode = new SqlParameter();
                returnCode.ParameterName = "@GroupID ";
                returnCode.SqlDbType = SqlDbType.Int;
                returnCode.Direction = ParameterDirection.Output;

                db.Database.ExecuteSqlCommand("sp_GetGroupePrincipalId @GroupID OUTPUT", returnCode);

                var type = returnCode.Value.GetType().FullName;

                if (type != "System.DBNull")
                { 
                    int groupId = (int)returnCode.Value;
                    List<Category> categories = db.Database.SqlQuery<Category>
                    ("sp_GetCategory @p0", groupId)
                    .ToList();
                    ViewBag.listeCategories = categories;
                }

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
                Category selectedCategory = db.Categories.Where(cat => cat.Id == categorieToDelete).FirstOrDefault();
                if (!selectedCategory.IsBase && selectedCategory != null)
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
using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();
        
        #region Get all Categories
        /// <summary>
        /// Index strana, pozivamo sve kategorije i subKategorije
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();

            return View();
        }
        #endregion

        #region Add new Category
        /// <summary>
        /// Kreiramo kategoriju proizvoda
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Category category)
        {
            var valNewCategory = db.tbl_Category.Where(c => c.Category_Name == category.Category_Name).FirstOrDefault();
            if (category.Category_Name != null)
            {
                if (valNewCategory == null)
                {

                    if (ModelState.IsValid)
                    {

                        category.ParentId = null;

                        db.tbl_Category.Add(category);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    TempData["Message_Error"] = "Kategorija postoji u Bazi!";
                    return View(category);
                }
            }
            TempData["Message_Error"] = "Morate uneti sve podatke!";
            return View(category);
        }
        #endregion



        #region Delete Category and SubCategory
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Category category = db.tbl_Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.tbl_Category.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        #endregion

        public ActionResult Edit(int? id)
        {
            var category = db.tbl_Category.Find(id);

            return View("Edit", category);
        }

        [HttpPost]
        public ActionResult Edit(int id, tbl_Category category)
        {
            var currentCategory = db.tbl_Category.Find(id);
            currentCategory.Category_Name = category.Category_Name;
            currentCategory.Category_Image = category.Category_Image;
            currentCategory.Status = category.Status;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        [HttpGet]
       
        public ActionResult AddSubCategory(int id)
        {
            var subcategory = new tbl_Category()
            {
                ParentId = id
            };
            return View("AddSubCategory", subcategory);
        }

        [HttpPost]

        public ActionResult AddSubCategory(int ParentId, tbl_Category subcategory)
        {
            db.tbl_Category.Add(subcategory);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SubCategoryDetails(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            ViewBag.subCategories = db.tbl_Category.Where(x => x.ParentId == id).ToList();

            return View();
        }
    }
}
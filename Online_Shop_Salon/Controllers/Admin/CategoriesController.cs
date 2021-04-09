using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();

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
                        string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                        string extension = Path.GetExtension(category.ImageFile.FileName);   // dodali smo u model tbl_CAtegory property ImageFile za input name
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        category.Category_Image = "/content/img/kategorije/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/content/img/kategorije/"), fileName);
                        category.ImageFile.SaveAs(fileName);
                      

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

        #region Edit Category admin panel

        public ActionResult Edit(int? id)
        {
            var category = db.tbl_Category.Find(id);

            return View("Edit", category);
        }

        [HttpPost]
        public ActionResult Edit(int id, tbl_Category category)
        {
            if (ModelState.IsValid)
            {
                var currentCategory = db.tbl_Category.Find(id);
                if (category.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(category.ImageFile.FileName);
                    string extension = Path.GetExtension(category.ImageFile.FileName);   // dodali smo u model tbl_CAtegory property ImageFile za input name
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    category.Category_Image = "/content/img/kategorije/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/content/img/kategorije/"), fileName);
                    category.ImageFile.SaveAs(fileName);
                    currentCategory.Category_Image = category.Category_Image;
                }
                
                
                currentCategory.Category_Name = category.Category_Name;
                currentCategory.Description= category.Description;
                currentCategory.Status = category.Status;

                db.SaveChanges();
                return RedirectToAction("Index");

            }
            
            return RedirectToAction("Index");


        }
        #endregion

        #region ADD Sub Category Admin Panel
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
            if (ModelState.IsValid)
            {
                var valNewCategory = db.tbl_Category.Where(c => c.Category_Name == subcategory.Category_Name).FirstOrDefault();
                if (valNewCategory == null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(subcategory.ImageFile.FileName);
                    string extension = Path.GetExtension(subcategory.ImageFile.FileName);   // dodali smo u model Photo property ImageFile za input name
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    subcategory.Category_Image = "/content/img/kategorije/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/content/img/kategorije/"), fileName);
                    subcategory.ImageFile.SaveAs(fileName);
                    db.tbl_Category.Add(subcategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message_Error"] = "Kategorija ili Podkategorija postoje u Bazi!";
                    return View(subcategory);
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Get Details for Category UserPanel
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SubCategoryDetails(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.subCategories = db.tbl_Category.Where(x => x.ParentId == id && x.Status == true).ToList();

            return View();
        }
        #endregion
    }
}
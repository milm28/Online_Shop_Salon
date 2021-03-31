using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    
    public class ProductsController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region List Product for Admin Index Page 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Products = db.tbl_Product.ToList();
            return View();
        }
        #endregion

        #region Create Product Admin Panel
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var check = db.tbl_Store.Where(c => c.Status == true).FirstOrDefault();
            if (check != null)
            {
                var category_id = db.tbl_Category.Where(x => x.ParentId != null);
                var Category_Id = new SelectList(category_id, "Category_Id", "Category_Name");
                ViewBag.Category_Id = Category_Id;
                ViewBag.StoreId = new SelectList(db.tbl_Store, "Store_Id", "Store_Name");
            }
            else
            {
                TempData["Message_Product_Error"] = "Trenutno nemate ni jednu prodavnicu!";
                return RedirectToAction("index","Products");
            }
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Product tbl_Product)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Product.Add(tbl_Product);
                db.SaveChanges();

                var defaoultPhoto = new tbl_Photo
                {
                    Image_Name = "/ImgProizvodi/no-img.jpg",
                    Status = false,
                    Product_Id = tbl_Product.Product_Id,
                    Main_Image = true
                };
                db.tbl_Photo.Add(defaoultPhoto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.tbl_Category, "Category_Id", "Category_Name", tbl_Product.Category_Id);
            ViewBag.StoreId = new SelectList(db.tbl_Store, "Store_Id", "Store_Name", tbl_Product.StoreId);
            return View(tbl_Product);
        }
        #endregion

        #region Edit Product from Admin Panel
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Product tbl_Product = db.tbl_Product.Find(id);
            if (tbl_Product == null)
            {
                return HttpNotFound();
            }
            var category_id = db.tbl_Category.Where(x => x.ParentId != null);
            var Category_Id = new SelectList(category_id, "Category_Id", "Category_Name", tbl_Product.Category_Id);
            ViewBag.Category_Id = Category_Id;
            ViewBag.StoreId = new SelectList(db.tbl_Store, "Store_Id", "Store_Name");
            return View(tbl_Product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Product tbl_Product)
        {
            if (ModelState.IsValid)
            {
                tbl_Product.Status = tbl_Product.Status;
                db.Entry(tbl_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.tbl_Category, "Category_Id", "Category_Name", tbl_Product.Category_Id);
            ViewBag.StoreId = new SelectList(db.tbl_Store, "Store_Id", "Store_Name", tbl_Product.StoreId);
            return View(tbl_Product);
        }
        #endregion

        #region Delete Product From Admin Panel
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.tbl_Product.Find(id);
            var photo = db.tbl_Photo.Where(x => x.Product_Id == product.Product_Id).ToList();
            if (product == null || photo == null)
            {
                return HttpNotFound();
            }
            else
            {
                foreach (var item in photo)
                {
                    db.tbl_Photo.Remove(item);
                    db.SaveChanges();
                }
                db.tbl_Product.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Product Details UserPage
        public ActionResult Details(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();

            var product = db.tbl_Product.Find(id);
            var mainImage = product.tbl_Photo.SingleOrDefault(p => p.Status && p.Main_Image);
            ViewBag.Product = product;
            ViewBag.MainImage = mainImage == null ? "no-image.jpg" : mainImage.Image_Name;
            return View();
        }
        #endregion

        #region ProductDetails for User Page
        public ActionResult ProductDetails(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            var product = db.tbl_Product.Where(x => x.Category_Id == id).ToList();

            ViewBag.Products = product;

            return View();
        }
        #endregion

        #region Search Product from User Page Layout

        [HttpGet]
        public ActionResult Search(string keyword)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            var products = db.tbl_Product.Where(p => p.Product_Name.Contains(keyword) && p.Status != false && p.Status != null).ToList();
            ViewBag.Products = products;
            ViewBag.Keyword = keyword;
            ViewBag.CountProducts = products.Count;
            return View();
        }
        #endregion
    }
}
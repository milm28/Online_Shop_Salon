﻿using Online_Shop_Salon.Models;
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
           
                var category_id = db.tbl_Category.Where(x => x.ParentId != null && x.Status==true);
                var Category_Id = new SelectList(category_id, "Category_Id", "Category_Name");
                ViewBag.Category_Id = Category_Id;
                var storeId = db.tbl_Store.Where(s =>s.Status == true);
                var StoreId = new SelectList(storeId, "Store_Id", "Store_Name");
                ViewBag.StoreId = StoreId;
            
                return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Product tbl_Product)
        {
            var valNewProduct = db.tbl_Product.Where(c => c.Product_Name == tbl_Product.Product_Name).FirstOrDefault();
            var category_id = db.tbl_Category.Where(x => x.ParentId != null && x.Status == true);
            var Category_Id = new SelectList(category_id, "Category_Id", "Category_Name");
            ViewBag.Category_Id = Category_Id;
            var storeId = db.tbl_Store.Where(s => s.Status == true);
            var StoreId = new SelectList(storeId, "Store_Id", "Store_Name");
            ViewBag.StoreId = StoreId;
            if (ModelState.IsValid)
            {
                if (valNewProduct == null)
                {

                    //tbl_Product.Status = false;
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
                else
                {
                    TempData["Message_Error"] = "Proizvod vec postoji u Bazi!";
                    ViewBag.Category_Id = Category_Id;
                    ViewBag.StoreId = StoreId;
                    return View(tbl_Product);
                }


            }

            ViewBag.Category_Id = Category_Id;
            ViewBag.StoreId = StoreId;
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
            var category_id = db.tbl_Category.Where(x => x.ParentId != null && x.Status == true);
            var Category_Id = new SelectList(category_id, "Category_Id", "Category_Name", tbl_Product.Category_Id);
            ViewBag.Category_Id = Category_Id;
            ViewBag.StoreId = new SelectList(db.tbl_Store.Where(x=>x.Status == true), "Store_Id", "Store_Name", tbl_Product.StoreId);
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
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
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
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            var product = db.tbl_Product.Where(x => x.Category_Id == id && x.Status==true && x.Quantity_Stock > 0).ToList();

            ViewBag.Products = product;

            return View();
        }
        #endregion

        #region Search Product from User Page Layout

        [HttpGet]
        public ActionResult Search(string keyword)
        {
            if (keyword != null && keyword != "")
            {
                //viewbag za dropdown iz navbar-a
                ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
                ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
                //var products = db.tbl_Product.Where(p => p.Product_Name.Contains(keyword) || p.tbl_Category.Category_Name.Contains(keyword) && (p.Status != false && p.tbl_Category.Status != null)).ToList();
                var products = db.tbl_Product.Where(p => p.Product_Name.Contains(keyword)&& p.Status != false).ToList();
                var categoriesSearch = db.tbl_Category.Where(c => c.Category_Name.Contains(keyword)).ToList();
                ViewBag.CategoriesSearch = categoriesSearch;
                ViewBag.Products = products; 
                ViewBag.Keyword = keyword;
                ViewBag.CountProducts = products.Count;
                return View();
            }
            return RedirectToAction("index","Home");
        }
        #endregion

        #region Get Product from Store
        /// <summary>
        /// Store Id for product in stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductDetailsFromStore(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            var productFromStore = db.tbl_Product.Where(s => s.StoreId == id && s.Status == true).ToList();

            ViewBag.ProductFromStore = productFromStore;

            return View();
        }

        #endregion
    }
}
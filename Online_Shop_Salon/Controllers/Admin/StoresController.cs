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
    [Authorize(Roles = "Admin")]
    public class StoresController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Get List for Admin panel
        public ActionResult Index()
        {
            ViewBag.Stores = db.tbl_Store.ToList();
            return View();
        }
        #endregion

        #region Get DetailsList for Admin panel
        [HttpGet]
        public ActionResult Details(int? id)
        {
            var StoresDetails = db.tbl_Store.Find(id);
        
            
            return View(StoresDetails);
        }
        #endregion

        #region Create Store from Admin Panel
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
      
        [ValidateAntiForgeryToken]
        public ActionResult Create( tbl_Store tbl_Store)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Store.Add(tbl_Store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Store);
        }
        #endregion

        #region Edit Store from Admin Panel
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Store tbl_Store = db.tbl_Store.Find(id);
            if (tbl_Store == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Store);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tbl_Store tbl_Store)
        {
            if (ModelState.IsValid)
            {
                var checkProduct = db.tbl_Product.Where(p => p.StoreId == tbl_Store.Store_Id).FirstOrDefault();
                if (checkProduct != null && tbl_Store.Status != true)
                {
                    
                    ModelState.AddModelError("Status", "Postoji Proizvod u magacinu, ne mozete da izbrisete Prodavnicu!");
                    return View(tbl_Store);

                }
                else
                {
                    db.Entry(tbl_Store).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

              
            }
            return View(tbl_Store);
        }

        #endregion

        #region OffLine status Store
        /// <summary>
        /// Offline store
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult OffLine(int? id)
        {
             tbl_Store store = db.tbl_Store.Find(id);
            store.Status = false;   
             //db.tbl_Store.Remove(store);
             db.SaveChanges();
             return RedirectToAction("Index");
        }
        #endregion

        
    }
}
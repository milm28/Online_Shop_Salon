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

       
        public ActionResult Index()
        {
            ViewBag.Stores = db.tbl_Store.ToList();
            return View();
        }

        
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
                db.Entry(tbl_Store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Store);
        }


        #region Delete SlideShow
        /// <summary>
        /// Delete SlideShow Image from dataBase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            
           
                tbl_Store store = db.tbl_Store.Find(id);
                db.tbl_Store.Remove(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            

        }
        #endregion
    }
}
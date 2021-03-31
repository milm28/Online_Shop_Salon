using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class SlideShowsController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Get Index page SlideShow 
        /// <summary>
        /// Koristimo za ucitavanje podataka(slider)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.SliderShows = db.SlideShows.ToList();
            return View();
        }
        #endregion

        #region Create/index page SlideShow
        /// <summary>
        /// Poziv stranice sa poljima za unos 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var slideShow = new SlideShow();
            return View("Create", slideShow);
        }
        #endregion

        #region Create/Post page SlideShow
        /// <summary>
        /// Insert u bazu i SaveAs img u folder
        /// </summary>
        /// <param name="slideShow"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SlideShow slideShow)
        {
            string fileName = Path.GetFileNameWithoutExtension(slideShow.ImageFile.FileName);
            string extension = Path.GetExtension(slideShow.ImageFile.FileName); // imageFile krejiran property u modelu slideShow
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            slideShow.Name = "../ImageSlider/" + fileName;
            fileName = Path.Combine(Server.MapPath("../ImageSlider/"), fileName);
            slideShow.ImageFile.SaveAs(fileName);
            db.SlideShows.Add(slideShow);
            db.SaveChanges();
            return RedirectToAction("index","slideShows");
        }
        #endregion

        #region Delete SlideShow
        /// <summary>
        /// Delete SlideShow Image from dataBase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            SlideShow slideShow = db.SlideShows.Find(id);
            db.SlideShows.Remove(slideShow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit Slider
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlideShow slideShow = db.SlideShows.Find(id);
            if (slideShow == null)
            {
                return HttpNotFound();
            }
            return View(slideShow);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SlideShow slideShow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(slideShow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slideShow);
        }
        #endregion
    }
}
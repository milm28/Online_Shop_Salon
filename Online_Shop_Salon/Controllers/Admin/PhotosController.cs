using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class PhotosController : Controller
    {

        private Online_shopEntities db = new Online_shopEntities();


        #region Get Metod for Image
        /// <summary>
        /// Preko Get metode uzimamo Product_Id, ispis svih slika za proizvod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            ViewBag.Product = db.tbl_Product.Find(id);
            ViewBag.Photo = db.tbl_Photo.Where(p => p.Product_Id == id).ToList();
            return View();
        }
        #endregion

        #region Create Image 
        /// <summary>
        /// Dodajemo slike za proizvod
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Create(int id)
        {
            ViewBag.Product_Id = db.tbl_Product.Find(id);
            var photo = new tbl_Photo
            {
                Product_Id = id
            };
            return View("Create", photo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Product_Id, tbl_Photo photo)
        {

            if (photo.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(photo.ImageFile.FileName);
                string extension = Path.GetExtension(photo.ImageFile.FileName);   // dodali smo u model Photo property ImageFile za input name
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                photo.Image_Name = "/ImgProizvodi/" + fileName;
                fileName = Path.Combine(Server.MapPath("/ImgProizvodi/"), fileName);
                photo.ImageFile.SaveAs(fileName);
                db.tbl_Photo.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Product_Id });
            }
            else
            {
                TempData["Message_Photo_Error"] = "Morate Uneti Sliku!";
                return RedirectToAction("create", new { id = Product_Id });
            }


        }
        #endregion

        #region SetMainImage 
        /// <summary>
        /// Saljemo true ako kliknemo na set main, proizvod dobija glavnu(main sliku)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetMainImage(int id, int productId)
        {
            var product = db.tbl_Product.Find(productId);
            product.tbl_Photo.ToList().ForEach(p =>
            {
                p.Main_Image = false;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
            });
            var photo = db.tbl_Photo.Find(id);
            photo.Main_Image = true;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = productId });
        }
        #endregion

        #region Delete/Photo
        /// <summary>
        /// brisemo sliku
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            tbl_Photo tbl_Photo = db.tbl_Photo.Find(id);
            db.tbl_Photo.Remove(tbl_Photo);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = tbl_Photo.Product_Id });
        }
        #endregion

        #region SetStatus Image
        [HttpGet]
        public ActionResult SetStatus(int id)
        {
            var photo = db.tbl_Photo.Find(id);
            if (photo.Status == true ? photo.Status = false : photo.Status = true);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = photo.Product_Id });
        }
        #endregion
    }
}
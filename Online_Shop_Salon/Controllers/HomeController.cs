using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers
{
    public class HomeController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Home page/index
        /// <summary>
        /// for slider and dropdown category
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ///Problem sa slider modelom, kada prebacim projekat na drugi racunar
            // var slideShows = db.SlideShows.Where(s => s.Status == true).ToList();
            // ViewBag.slideShows = slideShows;
            return View();
        }
        #endregion

    }
}
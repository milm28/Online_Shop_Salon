using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {       
        private Online_shopEntities db = new Online_shopEntities();

        #region DashBoard Index Page
        public ActionResult Index()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.accountCount = db.tbl_Account.Where(a => a.Status == true && a.Role_Id==1).ToList().Count;
            ViewBag.storeCount = db.tbl_Store.Where(a => a.Status == true).ToList().Count;
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        #endregion
    }
}
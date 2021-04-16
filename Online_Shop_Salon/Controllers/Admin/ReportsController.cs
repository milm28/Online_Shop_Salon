using Online_Shop_Salon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    { 
        private Online_shopEntities db = new Online_shopEntities();
        // GET: Reports
        public ActionResult Index()
        {

            List<tbl_Invoice_Detail> allReportStores = db.tbl_Invoice_Detail.Where(t=>t.tbl_Invoice.Status==true).ToList();
            ViewBag.AllReportStores = allReportStores;
            return View();
        }
    }
}
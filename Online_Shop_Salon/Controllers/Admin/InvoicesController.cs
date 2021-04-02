using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Shop_Salon.Models;

namespace Online_Shop_Salon.Controllers.Admin
{
    
    public class InvoicesController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Index Page for Admin Panel
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Invoices = db.tbl_Invoice.Include(t => t.tbl_Account).ToList();
            
            return View();
        }
        #endregion

        #region List Invoice for user page
        [HttpGet]
        [Authorize(Roles="User")]
        public ActionResult ListInvoice(int id)
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            //ViewBag.Invoices = db.tbl_Invoice.Include(t => t.tbl_Account).ToList();
            ViewBag.Invoices = db.tbl_Invoice.Where(x => x.Account_Id == id).ToList();
                    
            return View();
        }
        #endregion

        #region Details for Invoice user page
        [Authorize(Roles = "User")]
        public ActionResult InvoiceDetailsUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            var invoicesDetails = db.tbl_Invoice_Detail.Where(i => i.Invoice_Id == id).ToList();
            ViewBag.InvoicesDetails = invoicesDetails;

            return View("InvoiceDetailsUser");
        }
        #endregion

        #region Details Invoice for Admin Page
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var invoicesDetails = db.tbl_Invoice_Detail.Where(i => i.Invoice_Id == id).ToList();
            ViewBag.InvoicesDetails = invoicesDetails;

            return View("Details");
        }
        #endregion

        #region Set Status Payment Admin Page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult SetStatus(int id)
        {
            var invoice = db.tbl_Invoice.Find(id);
            if (invoice.Status == true ? invoice.Status = false : invoice.Status = true);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = invoice.Invoice_Id });
        }
        #endregion


    }
}

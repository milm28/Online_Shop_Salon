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
            
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            //ViewBag.Invoices = db.tbl_Invoice.Include(t => t.tbl_Account).ToList();
            ViewBag.Invoices = db.tbl_Invoice.Where(x => x.Account_Id == id).ToList();
                var invoiceAccount = db.tbl_Invoice.Where(x => x.Account_Id == id).FirstOrDefault();
            ViewBag.InvoiceAccount = invoiceAccount;
            if (invoiceAccount == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                TempData["Message_Invoice_Error"] = "Nema te ni jedan racun";
                return View();
            }
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
           
            ViewBag.InvoiceData = db.tbl_Invoice_Detail.Where(d => d.Invoice_Id == id).FirstOrDefault();
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true ).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
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
            ViewBag.InvoiceData = db.tbl_Invoice_Detail.Where(d => d.Invoice_Id == id).FirstOrDefault();
            var invoicesDetails = db.tbl_Invoice_Detail.Where(i => i.Invoice_Id == id).ToList();
            ViewBag.InvoicesDetails = invoicesDetails;

            return View("Details");
        }
        #endregion

        #region Set Status Payment Admin Page
        /// <summary>
        /// Kada se setuje da je faktura placena, umanjujemo kolicinu za traj proizvod u magacinu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult SetStatus(int id)
        {
            
            
            var invoice = db.tbl_Invoice.Find(id);
            if (invoice.Status == true ? invoice.Status = false : invoice.Status = true);
            db.SaveChanges();
            var invoicesDetails = db.tbl_Invoice_Detail.Where(i => i.Invoice_Id == id).ToList();
            foreach(var quantityProduct in invoicesDetails)
            {
                var productId = db.tbl_Product.Where(x => x.Product_Id == quantityProduct.Product_Id).FirstOrDefault();
                var qq=(productId.Quantity_Stock - quantityProduct.Quantity);

                var quantityFromProduct = db.tbl_Product.Where(x => x.Product_Id == quantityProduct.Product_Id && x.StoreId == quantityProduct.Store_Id).FirstOrDefault();
                quantityFromProduct.Quantity_Stock = qq;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = invoice.Invoice_Id });
        }
        #endregion


    }
}

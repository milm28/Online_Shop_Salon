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
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Index page for list account (Admin Panel)
        public ActionResult Index()
        {
            var tbl_Account = db.tbl_Account.Include(t => t.tbl_Role);
            return View(tbl_Account.ToList());
        }
        #endregion

        #region Create New User or Admin from Admin ControlPanel
        public ActionResult Create()
        {
            ViewBag.Role_Id = new SelectList(db.tbl_Role, "Role_Id", "Role_Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_Account tbl_Account)
        {
            if (ModelState.IsValid)
            {
                tbl_Account.Password = Encode(tbl_Account.Password);
                db.tbl_Account.Add(tbl_Account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Role_Id = new SelectList(db.tbl_Role, "Role_Id", "Role_Name", tbl_Account.Role_Id);
            return View(tbl_Account);
        }
        #endregion

        #region Edit User or Admin from Admin ControlPanel
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbl_Account tbl_Account = db.tbl_Account.Find(id);
            if (tbl_Account == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.Role_Id = new SelectList(db.tbl_Role, "Role_Id", "Role_Name", tbl_Account.Role_Id);
            return View(tbl_Account);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tbl_Account tbl_Account)
        {
            if (ModelState.IsValid)
            {
                tbl_Account.Password = Encode(tbl_Account.Password);
                db.Entry(tbl_Account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role_Id = new SelectList(db.tbl_Role, "Role_Id", "Role_Name", tbl_Account.Role_Id);
            return View(tbl_Account);
        }

        #endregion

        #region Encoding password
        /// <summary>
        /// encoding za registraciju i logovanje 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encode(string password)
        {
            try
            {
                byte[] EncDataByte = new byte[password.Length];
                EncDataByte = System.Text.Encoding.UTF8.GetBytes(password);
                string EncryptedData = Convert.ToBase64String(EncDataByte);
                return EncryptedData;

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Encode:" + ex.Message);
            }
        }
        #endregion



    }
}

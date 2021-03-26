using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Online_Shop_Salon.Models;

namespace Online_Shop_Salon.Controllers
{
    public class LoginController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();
        // GET: Login
        public ActionResult Register()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Register(tbl_Account user)
        {
           
                user.Role_Id = 2;
                db.tbl_Account.Add(user);
                db.SaveChanges();
                ModelState.Clear();

            TempData["Message"] = "Uspesno ste se registrovali!";
            return RedirectToAction("Index", "Home");
        }

        #region Login Get and Post
        public ActionResult Login()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
            using (Online_shopEntities db = new Online_shopEntities())
            {
                var user = db.tbl_Account.Where(a => a.Email == login.Email && a.Password == login.Password).FirstOrDefault();
                if (user != null)
                {
                    var Ticket = new FormsAuthenticationTicket(login.Email, true, 3000);
                    string Encrypt = FormsAuthentication.Encrypt(Ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
                    cookie.Expires = DateTime.Now.AddHours(3000);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    if (user.Role_Id == 1)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }
        #endregion

        #region LogOut
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
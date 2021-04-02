using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Online_Shop_Salon.Models;
using System.Security.Cryptography;

namespace Online_Shop_Salon.Controllers
{
    public class LoginController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region Register Page
        public ActionResult Register()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        #endregion

        #region Register new user(only user)
        [HttpPost]
        public ActionResult Register(tbl_Account user)
        {
            var check = db.tbl_Account.FirstOrDefault(s => s.Email == user.Email);
            if (check == null)
            {
                user.Role_Id = 2;
                user.Status = true;
                db.tbl_Account.Add(user);
                db.SaveChanges();
                ModelState.Clear();

                TempData["Message_Register_Success"] = "Uspesno ste se registrovali!";
                return RedirectToAction("Login", "Login");
            }
            TempData["Message_Register_Error"] = "Email postoji u bazi!";
            return RedirectToAction("Register", "login");
        }
        #endregion

        #region Login Page
        public ActionResult Login()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        #endregion

        #region Login User/Admin multiple Login
        [HttpPost]
        public ActionResult Login(Login login)
        {
  
            var user = db.tbl_Account.Where(a => a.Email == login.Email && a.Password == login.Password && a.Status!=false).FirstOrDefault();

            if (user != null)
            {
               
                ///za ispis na layoutu
                HttpCookie UserCookieName = new HttpCookie("user_cookie_name", user.UserName.ToString());
                UserCookieName.Expires.AddDays(10);
                HttpContext.Response.SetCookie(UserCookieName);
                HttpCookie UserCookieId = new HttpCookie("user_cookie_id", user.Account_Id.ToString());
                UserCookieId.Expires.AddDays(10);
                HttpContext.Response.SetCookie(UserCookieId);
                ///end za ispis

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
                            //return Redirect(Request.UrlReferrer.ToString());
                }
            }
            TempData["Message_Login_Error"] = "Kupac ne postoji u bazi!";
            return RedirectToAction("Login", "Login");
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
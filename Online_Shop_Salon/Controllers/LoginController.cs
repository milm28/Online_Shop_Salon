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
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        #endregion

        #region Register new user(only user)
        [HttpPost]
        public ActionResult Register(tbl_Account user)
        {
            var check = db.tbl_Account.FirstOrDefault(s => s.Email == user.Email || s.UserName == user.UserName);
            if (check == null)
            {

                user.Role_Id = 2;
                user.Status = true;
                user.Password = Encode(user.Password);
                db.tbl_Account.Add(user);
                db.SaveChanges();
                ModelState.Clear();

                TempData["Message_Register_Success"] = "Uspesno ste se registrovali!";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                TempData["Message_Register_Error"] = "Email/UserName postoji u bazi!";
                return RedirectToAction("Register", "login");
                
            }
            
        }
        #endregion

        #region Login Page
        public ActionResult Login()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            ViewBag.products = db.tbl_Product.ToList();
            return View();
        }
        #endregion

        #region Login User/Admin multiple Login
        [HttpPost]
        public ActionResult Login(Login login)
        {
            
            var passEncrypt= Encode(login.Password);
            var user = db.tbl_Account.Where(a => a.Email == login.Email && a.Password == passEncrypt && a.Status!=false).FirstOrDefault();

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
            TempData["Message_Login_Error"] = "Email ne postoji u Bazi!";
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
            catch(Exception ex)
            {
                throw new Exception ("Error in Encode:" + ex.Message);
            }
        }
        #endregion

    }

}
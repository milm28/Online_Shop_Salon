using Online_Shop_Salon.Models;
using Online_Shop_Salon.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers
{
    public class CartShopController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();

        #region CheckDetails Page for product
        public ActionResult CheckoutDetails()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            return View();
        }
        #endregion

        #region AddtoCart product
        /// <summary>
        /// using for add new product and increase product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddToCart(int id)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = db.tbl_Product.Find(id);
                cart.Add(new Item()
                {
                    
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
               // return RedirectToAction("../Products/Details/", new { id = id });
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = db.tbl_Product.Find(id);
                List<int> addedItems = new List<int>();
                foreach (var item in cart)
                {
                    addedItems.Add(item.Product.Product_Id);
                }
                foreach (var item in cart.ToList())
                {
                    if (item.Product.Product_Id == id)
                    {
                        int prevQty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = (prevQty + 1)
                        });
                        Session["cart"] = cart;
                    }
                    else
                    {
                        if (addedItems.Contains(id))
                        {

                        }
                        else
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                            Session["cart"] = cart;
                        }                       
                    }
                }
                //return RedirectToAction("../Products/Details/", new { id = id });
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        #endregion

        #region Decrease Quantity product from cart
        /// <summary>
        /// decrease product from product details page (icons -)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteQty(int id)
        {
            if(Session ["cart"] != null)
            {
                List<Item> cart = (List <Item>)Session["cart"];
                var product = db.tbl_Product.Find(id);
                foreach(var item in cart)
                {
                    if(item.Product.Product_Id == id)
                    {
                        int prQty = item.Quantity;
                        if(prQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prQty - 1
                            });
                        }
                        //return Redirect(Request.UrlReferrer.ToString());
                        break;
                    }             
                }
                Session["cart"] = cart;
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region Delete Product from cart
        /// <summary>
        /// delete product from cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult RemoveFromCart(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart.ToList())
            {
                if (item.Product.Product_Id == id)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = null;
            //return RedirectToAction("../Products/Details/", new { id = id });
            return Redirect(Request.UrlReferrer.ToString());

        }
        #endregion

        #region CheckOut Create Invoice
        /// <summary>
        /// posle pagedetails poziva se checkout koji upisuje podatek u tbl_Invoice i tbl_invoice_detal
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult CheckOut(int? userId)
        {
            
            var customer = db.tbl_Account.FirstOrDefault(x => x.Account_Id == userId);
            if (userId != null)
            {
                //cREATE INVOICE
                var invoice = new tbl_Invoice()
                {

                    Created = DateTime.Now,
                    Status = true,
                    Account_Id = customer.Account_Id
                };
                db.tbl_Invoice.Add(invoice);
                db.SaveChanges();

                //invoice details
                
                foreach (Item item in (List<Item>)Session["cart"])
                {
                    var invoiceDetails = new tbl_Invoice_Detail
                    {
                        Invoice_Id = invoice.Invoice_Id,
                        Product_Id = item.Product.Product_Id,
                        Quantity = item.Quantity,
                        Tax = 20,
                        Price_Per = item.Product.Price_Per,
                        Total_Price_Tax = (item.Product.Price_Per * 80/100)*item.Quantity

                    };
                    db.tbl_Invoice_Detail.Add(invoiceDetails);
                    db.SaveChanges();
                    Session["cart"] = null;
                }

                return RedirectToAction("Thanks", "CartShop");
            }
            return RedirectToAction("Login", "login");
        }
        #endregion

        #region Thank You Page
        /// <summary>
        /// posle uspesno zavresene narudzbine, poziva se thanks Page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        public ActionResult Thanks()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            return View();
        }
        #endregion
    }
}
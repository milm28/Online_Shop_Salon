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
            if (Request.Cookies["user_cookie_name"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
           
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            return View();
        }
        #endregion

        #region AddtoCart product
        /// <summary>
        /// using for add new product and increase product, this is a product ID in param
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddToCart(int id)
        {
            var validateQuantityProduct = db.tbl_Product.Where(q => q.Product_Id == id && q.Status == true).FirstOrDefault();

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
                    ///TempData["Message_Quantity_Product_Error"] = "Trenutno nemamo više proizvoda na stanju!";
                    ///Provera kolicine u bazi tj.magacinu za odredjeni proizvod, ako je kolicina iz cart veca od kolicine iz magacina ide break; 
                    
                        
                        if (item.Product.Product_Id == id)
                        {
                            if (item.Quantity < product.Quantity_Stock)
                            {
                                int prevQty = item.Quantity;
                                cart.Remove(item);
                                cart.Add(new Item()
                                {
                                    Product = product,
                                    Quantity = (prevQty + 1)
                                });
                            }
                            else
                            {
                                TempData["Message_Quantity_Product_Error"] = "Trenutno nemamo više proizvoda na stanju!";
                            }
                            Session["cart"] = cart.OrderBy(s => s.Product.Product_Name).ToList();
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
                                    break;
                                
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
                        if(prQty > 1)
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
                Session["cart"] = cart.OrderBy(s => s.Product.Product_Name).ToList();
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
            if(cart.Count == 0)
            {
                Session["cart"] = null;
            }
            else
            {
                Session["cart"] = cart;
            }
            
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
       
        public ActionResult CheckOut(int? userId)
        {
            
            var customer = db.tbl_Account.FirstOrDefault(x => x.Account_Id == userId);
            if (userId != null && customer.Role_Id !=1)
            {
                //Kreiramo fakturu
                var invoice = new tbl_Invoice()
                {

                    Created = DateTime.Now,
                    Status = false,
                    Account_Id = customer.Account_Id
                };
                db.tbl_Invoice.Add(invoice);
                db.SaveChanges();

                //Detalji Fakture
                
                foreach (Item item in (List<Item>)Session["cart"])
                {
                    var invoiceDetails = new tbl_Invoice_Detail
                    {
                        Invoice_Id = invoice.Invoice_Id,
                        Product_Id = item.Product.Product_Id,
                        Store_Id = item.Product.StoreId,
                        Quantity = item.Quantity,
                        Tax = 20,
                        Price_Per = item.Product.Price_Per,
                        Total_Price_Tax = item.Product.Price_Per * item.Quantity

                    };
                    db.tbl_Invoice_Detail.Add(invoiceDetails);
                    db.SaveChanges();
                    Session["cart"] = null;
                }

                return RedirectToAction("Thanks", "CartShop");
            }
            TempData["Message_CheckOut_Error"] = "Morate da se ulogujete kao kupac!!";
      
            return RedirectToAction("Login", "Login");
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
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null && x.Status == true).ToList();
            ViewBag.SalonsList = db.tbl_Store.Where(x => x.Status == true).ToList();
            return View();
        }
        #endregion
    }
}
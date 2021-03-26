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
        public ActionResult Index()
        {
            return View();
        }
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
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = db.tbl_Product.Find(id);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            return View();
        }
    }
}
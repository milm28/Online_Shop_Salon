using Online_Shop_Salon.Helpers;
using Online_Shop_Salon.Models;
using Microsoft.AspNetCore.Http;
using Online_Shop_Salon.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Online_Shop_Salon.Controllers
{
    public class CartController : Controller
    {
        private Online_shopEntities db = new Online_shopEntities();
        public ActionResult Index()
        {
            ViewBag.categories = db.tbl_Category.Where(x => x.ParentId == null).ToList();
            List<Item> cart = (SessionHelper.GetObjectFromJson<List<Item>>((ISession)HttpContext.Session,"cart"));
            ViewBag.cart = cart;
            ViewBag.Total = cart.Sum(t => t.Product.Price_Per * t.Quantity);
            ViewBag.cartItemsCount = cart.Count;
            return View();
        }

        public ActionResult Buy(int id)
        {
            var product = db.tbl_Product.Find(id);
            if (SessionHelper.GetObjectFromJson<List<Item>>((ISession)HttpContext.Session,"cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item
                {
                    Product = product,
                    Quantity = 1
                });
                SessionHelper.SetObjectAsJson((ISession)HttpContext.Session, "cart", cart);
                int index = existes(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Product = product,
                        Quantity = 1
                    });
                }
                else
                {
                    int newQuantity = cart[index].Quantity++;
                    cart[index].Quantity = newQuantity;
                }
                SessionHelper.SetObjectAsJson((ISession)HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>((ISession)HttpContext.Session, "cart");
            }
            return RedirectToAction("Index", "Cart");
        }

        private int existes(int id,List<Item> cart)
        {
            for(var i=0;i<cart.Count; i++)
            {
                if (cart[i].Product.Product_Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
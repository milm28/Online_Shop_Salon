using Online_Shop_Salon.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Shop_Salon.Controllers
{
    public class ShopCartController : Controller
    {
        // GET: ShopCart
        public ActionResult Index()
        {
            return View();
        }
        
    }
}
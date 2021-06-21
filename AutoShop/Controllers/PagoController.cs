using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private contextAutoShop db = new contextAutoShop();
        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }
    }
}
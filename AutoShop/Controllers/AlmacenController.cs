using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    public class AlmacenController : Controller
    {
        contextAutoShop db = new contextAutoShop();
        // GET: Almacen
        public ActionResult Index()
        {
            var query = (from c in db.Envio
                         select c).ToList();
            var conteoEnviosHoy = 0;
            var conteoEnviosSemana = 0;
            var conteoEnviosAnio = 0;

            DateTime hoy = new DateTime();
            var hoyy = hoy.ToShortDateString();

            if (query.Count != 0)
            {
                foreach (Envio listado in (List<Envio>)query)
                {
                    DateTime hoyyy = new DateTime();
                    if (listado.fecha_envio.Equals(hoy.ToShortDateString()))
                    {

                    }
                }
            }
            return View();
        }
    }
}
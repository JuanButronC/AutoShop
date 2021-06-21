using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;
using System.Data.Entity;

namespace AutoShop.Controllers
{
    public class AdminstradorController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Administrador
        public ActionResult Index()
        {
            var administradores = db.Empleado.Include(e => e.Roles);
            ViewBag.total = administradores.Where(e => e.rol_fk != 3).Count();
            ViewBag.totalCompras = administradores.Where(e => e.rol_fk == 1).Count();
            ViewBag.totalAlmacen = administradores.Where(e => e.rol_fk == 2).Count();
            ViewBag.listaCompras = administradores.Where(e => e.rol_fk == 1).ToList();
            ViewBag.listaAlmacen = administradores.Where(e => e.rol_fk == 2).ToList();

            return View();
        }
    }
}
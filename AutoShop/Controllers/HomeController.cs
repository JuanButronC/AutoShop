using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    public class HomeController : Controller
    {
        private contextAutoShop db = new contextAutoShop();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name;

                Cliente cliente = (from c in db.Cliente
                                   where c.correo == correo
                                   select c).ToList().FirstOrDefault();

                if (cliente == null)
                {
                    Empleado empleado = (from c in db.Empleado
                                         where c.correo == correo
                                         select c).ToList().FirstOrDefault();
                    Session["name"] = empleado.nombre;
                    RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                }
                else
                {

                    Session["name"] = cliente.nombre;
                }
            }
            else
            {

                Session["name"] = "";
            }
            if (Session["itemTotal"] == null)
            {
                Session["itemTotal"] = 0;
            }

            List<Producto> producto = (from c in db.Producto
                                       where c.descuento > 0
                                       select c).ToList();
            int cuenta = producto.Count;

            if (cuenta >= 4)
            {
                ViewBag.producto = producto.GetRange(0, 4);
            }
            else
            {
                ViewBag.producto = producto.GetRange(0, cuenta);
            }


            List<Producto> novedades = (from c in db.Producto
                                        select c).ToList();
            int cuentas = novedades.Count;
            if (cuentas >= 9)
            {
                ViewBag.novedades = novedades.GetRange(0, 8);
            }
            else
            {
                ViewBag.novedades = novedades.GetRange(0, cuentas);
            }


            var comentarios = db.Comentarios.Include(c => c.Cliente);

            ViewBag.comentarios = comentarios.ToList();
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult getImg(int id)
        {
            Producto imgProd = (from i in db.Producto
                                 where i.id == id
                                 select i).ToList().FirstOrDefault();

            var fileToRetrieve = imgProd.imagen;
            if (fileToRetrieve == null)
            {
                return null;
            }
            else
            {
                return File(fileToRetrieve, "image/jpeg");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        //Contexto de base de datos
        private contextAutoShop db = new contextAutoShop();

        // GET: Usuario
        public ActionResult Index(string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = email;
                string rol = "";
                using (db)
                {
                    var query = from st in db.Empleado
                                where st.correo == correo
                                select st;
                    var lista = query.ToList();
                    if (lista.Count > 0)
                    {
                        var empleado = query.FirstOrDefault<Empleado>();
                        string[] nombres = empleado.nombre.ToString().Split(' ');
                        Session["name"] = nombres[0];
                        Session["usr"] = empleado.nombre;
                        rol = empleado.rol_fk.ToString().TrimEnd();

                        if (HttpContext.Request.Cookies["usuario"] == null)
                        {
                            HttpCookie cookie = new HttpCookie("usuario");
                            cookie["rol"] = rol;
                            cookie["name"] = Session["name"].ToString();
                            cookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(cookie);
                        }
                    }
                    else
                    {
                        var query1 = from st in db.Cliente
                                     where st.correo == correo
                                     select st;
                        var lista1 = query1.ToList();
                        if (lista1.Count > 0)
                        {
                            Cliente cliente = query1.FirstOrDefault<Cliente>();
                            string[] nombres = cliente.nombre.ToString().Split(' ');
                            Session["name"] = nombres[0];
                            Session["usr"] = cliente.nombre;
                            rol = "cliente";

                            if (HttpContext.Request.Cookies["usuario"] == null)
                            {
                                List<Item> cart = new List<Item>();
                                Session["cart"] = cart;

                                HttpCookie cookie = new HttpCookie("usuario");
                                cookie["rol"] = rol;
                                cookie["name"] = Session["name"].ToString();
                                cookie["itemTotal"] = "0";
                                cookie.Expires = DateTime.Now.AddDays(1);
                                Response.Cookies.Add(cookie);
                            }
                        }
                    }
                }
                if (rol == "1")
                {
                    return RedirectToAction("Index", "Compras");
                }
                if (rol == "2")
                {
                    return RedirectToAction("Index", "Almacen");
                }
                if (rol == "3")
                {
                    return RedirectToAction("Index", "Administrador");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
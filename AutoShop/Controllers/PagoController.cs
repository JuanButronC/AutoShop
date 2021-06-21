using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private contextAutoShop db = new contextAutoShop();
        private string NumConfirPago;
        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CrearOrden()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["CrearOrden"] = "pend";
                return RedirectToAction("Login", "Account");
            }

            string correo = User.Identity.Name;

            string fechaCreacion = DateTime.Today.ToShortDateString();
            string fechaProbEntrega = DateTime.Today.AddDays(3).ToShortDateString();

            var cliente = (from c in db.Cliente
                           where c.correo == correo
                           select c).ToList().FirstOrDefault();


            var tarjetas = (from c in db.Tarjeta
                            where c.id_cliente_fk == cliente.id
                            select c).ToList();
            int countT = tarjetas.Count;
            var direcciones = (from c in db.Direccion
                               where c.id_cliente_fk == cliente.id
                               select c).ToList();
            int countD = direcciones.Count;
            ViewBag.tarjetas = tarjetas;
            ViewBag.countD = countD;
            ViewBag.countT = countT;
            ViewBag.direcciones = direcciones;

            return View();
        }

        [HttpPost]
        public ActionResult Pagar(string tipo, int? dir, int? tar)
        {
            ViewBag.t = tipo;
            ViewBag.d = dir;
            ViewBag.ta = tar;


            string correo = User.Identity.Name;

            DateTime fc = DateTime.Today;
            DateTime fpe = fc.AddDays(3);
            var cliente = (from c in db.Cliente
                           where c.correo == correo
                           select c).ToList().FirstOrDefault();
            int ic = cliente.id;
            if (tipo.Equals("T"))
            {
                if (!validaPago(cliente))
                {
                    return RedirectToAction("pagoNoAceptado");
                }
                else
                {
                    int id = (int)dir;
                    return RedirectToAction("PagoAceptado", routeValues: new { idC = ic, idD = id, idTar = tar });
                }
            }

            if (tipo.Equals("P"))
            {
                int id = (int)dir;
                validaPago(cliente);
                return RedirectToAction("pagoPaypal", routeValues: new { idC = ic, idD = id, idTar = tar });

            }

            return View();
        }

        public ActionResult PagoAceptado(int idC, int idD, int idTar)
        {
            Orden oc = new Orden();

            oc.fecha_creacion = DateTime.Today;
            oc.numero_confirmacion = Convert.ToString(Session["nConfirma"]);
            var carro = Session["cart"] as List<Item>;
            var total = carro.Sum(item => item.Producto.precio * item.Cantidad);
            oc.total = (decimal)total;
            oc.id_cliente_fk = idC;
            oc.id_direccion_fk = idD;
            if (idTar == 0)
            {
                var tarjeta = (from c in db.Tarjeta
                               where c.id_cliente_fk == idC
                               select c).ToList().FirstOrDefault();
                oc.id_tarjeta_fk = tarjeta.id;
            }
            else
            {
                oc.id_tarjeta_fk = idTar;
            }

            db.Orden.Add(oc);
            db.SaveChanges();

            Orden_Producto op;
            List<Orden_Producto> lp = new List<Orden_Producto>();
            foreach (Item itm in carro)
            {
                op = new Orden_Producto();
                op.id_orden_fk = oc.id;
                op.id_prod_fk = itm.Producto.id;
                op.cantidad = itm.Cantidad;
                db.Orden_Producto.Add(op);
            }
            db.SaveChanges();

            Envio env = new Envio();
            env.id_cliente_fk = idC;
            env.id_direccion_fk = idD;
            env.id_orden_fk = oc.id;
            env.id_paq_fk = 1;
            db.Envio.Add(env);
            db.SaveChanges();

            List<Item> cart = new List<Item>();
            Session["cart"] = cart;
            Session["nConfrima"] = null;
            Session["itemTotal"] = 0;
            return View();
        }

        private bool validaPago(Cliente cliente)
        {
            bool retorna = true;

            int randomvalue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] val = new byte[6];
                crypto.GetBytes(val);
                randomvalue = BitConverter.ToInt32(val, 1);
            }

            NumConfirPago = Math.Abs(randomvalue * 1000).ToString();
            Session["nConfirma"] = NumConfirPago;

            return retorna;
        }

        public ActionResult pagoNoAceptado()
        {
            return View();
        }

        public ActionResult pagoPaypal(int idC, int idD)
        {
            Session["idDir"] = idD;
            Session["idClient"] = idC;
            return View();
        }

        public ActionResult pagandoPaypal(int idC, int idD)
        {
            Session["idDir"] = idD;
            Session["idClient"] = idC;
            return View();
        }
    }
}
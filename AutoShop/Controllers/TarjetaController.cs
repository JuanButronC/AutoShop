using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    public class TarjetaController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Tarjeta
        public ActionResult Index()
        {
            var tarjeta = db.Tarjeta.Include(t => t.Cliente);
            return View(tarjeta.ToList());
        }

        // GET: Tarjeta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta);
        }

        // GET: Tarjeta/Create
        public ActionResult Create()
        {

            return View();
        }

        // GET: Direccion
        public ActionResult MisTarjetas()
        {
            int id = 0;
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name;

                Cliente cliente = (from c in db.Cliente
                                   where c.correo == correo
                                   select c).ToList().FirstOrDefault();
                id = cliente.id;
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            var tarjetas = db.Tarjeta.Where(t => t.id_cliente_fk == id).Include(t => t.Cliente);
            Session["idCliente"] = id;
            return View(tarjetas.ToList());
        }

        // POST: Tarjeta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_cliente_fk,nombre,numero_cuenta,mes_vencimiento,anio_vencimiento,cvv,tipoTarj")] Tarjeta tarjeta)
        {

            if (Tarjeta(tarjeta.nombre, tarjeta.numero_cuenta, tarjeta.tipoTarj, tarjeta.mes_vencimiento, tarjeta.anio_vencimiento, tarjeta.cvv))
            {
                if (true)
                //TODO:SIMULAR METODO DE VALIDACION DE TARJETA
                {
                    /*Tarjeta tarj = new Tarjeta();
                    tarj.id_cliente_fk = (int)Session["idCliente"];
                    tarj.nombre = tarjeta.nombre;
                    tarj.numero_cuenta = tarjeta.numero_cuenta;
                    tarj.mes_vencimiento = tarjeta.mes_vencimiento;
                    tarj.anio_vencimiento = tarjeta.anio_vencimiento;
                    tarj.cvv = tarjeta.cvv;
                    tarj.tipoTarj = tarjeta.tipoTarj;*/


                    //db.Tarjeta.Add(tarj);
                    tarjeta.id_cliente_fk = (int)Session["idCliente"];
                    db.Tarjeta.Add(tarjeta);
                    db.SaveChanges();

                    if (Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend"))
                        {

                            return RedirectToAction("Create", "Direccion");
                        }

                    }
                    else
                    {
                        return RedirectToAction("MisTarjetas");
                    }
                }
                else
                {
                    return RedirectToAction("Invalida", new { esEdicion = false });
                }
            }
            else
            {
                return RedirectToAction("Invalida", new { esEdicion = false });
            }


            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", tarjeta.id_cliente_fk);
            return View(tarjeta);
        }



        private bool Tarjeta(string nombre, string numero_cuenta, int tipoTarj, int mes_vencimiento, int anio_vencimiento, int cvv)
        {
            bool retorna = validaTarj(numero_cuenta);
            if (retorna)
            {
                if ((numero_cuenta.StartsWith("4")) && (tipoTarj == 1))//visa
                {
                    retorna = true;
                }
                else if ((numero_cuenta.StartsWith("5")) && (tipoTarj == 2))//mastercard
                {
                    retorna = true;
                }
                else if ((numero_cuenta.StartsWith("3")) && (tipoTarj == 3))//americanexpress
                {
                    retorna = true;
                }
                else
                {
                    retorna = false;
                }

                DateTime hoy = new DateTime();

                if (Convert.ToInt32(anio_vencimiento) >= hoy.Year)
                {
                    if (Convert.ToInt32(mes_vencimiento) > hoy.Month)
                    {
                        retorna = true;
                    }
                    else
                    {
                        retorna = false;
                    }


                }
                else
                {
                    retorna = false;
                }

            }
            return retorna;
        }

        private bool validaTarj(string tarj)
        {
            bool retorna = true;
            StringBuilder digitsOnly = new StringBuilder();
            foreach (Char c in tarj)
            {
                if (Char.IsDigit(c)) digitsOnly.Append(c);
            };
            if (digitsOnly.Length > 18 || digitsOnly.Length < 15) return false;

            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                digit = Int32.Parse(digitsOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9) addend -= 9;
                }
                else
                {
                    addend = digit;
                }
                sum += addend;
                timesTwo = !timesTwo;
            }

            retorna = ((sum % 10) == 0);

            return retorna;
        }

        public ActionResult Invalida(bool esEdicion)
        {
            ViewBag.esEdicion = esEdicion;
            return View();
        }
        // GET: Tarjeta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", tarjeta.id_cliente_fk);
            return View(tarjeta);
        }

        // POST: Tarjeta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_cliente_fk,nombre,numero_cuenta,mes_vencimiento,anio_vencimiento,cvv,tipoTarj")] Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                if (Tarjeta(tarjeta.nombre, tarjeta.numero_cuenta, tarjeta.tipoTarj, tarjeta.mes_vencimiento, tarjeta.anio_vencimiento, tarjeta.cvv))
                {
                    if (true)
                    //TODO:SIMULAR METODO DE VALIDACION DE TARJETA
                    {
                        db.Entry(tarjeta).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("MisTarjetas");
                    }
                    else
                    {
                        return RedirectToAction("Invalida", new { esEdicion = true });
                    }
                }
                else
                {
                    return RedirectToAction("Invalida", new { esEdicion = true });
                }
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", tarjeta.id_cliente_fk);
            return View(tarjeta);
        }

        // GET: Tarjeta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }
            return View(tarjeta);
        }

        // POST: Tarjeta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            db.Tarjeta.Remove(tarjeta);
            db.SaveChanges();
            return RedirectToAction("MisTarjetas");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

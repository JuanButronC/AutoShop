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
    public class ClienteController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Cliente
        public ActionResult Index()
        {
            return View(db.Cliente.ToList());
        }


        // GET: Cliente/Details/5
        public ActionResult MisDatos()
        {
            Cliente cliente = new Cliente();
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name;

                cliente = (from c in db.Cliente
                           where c.correo == correo
                           select c).ToList().FirstOrDefault();
                int id = cliente.id;
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }

            return View(cliente);
        }


        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            string correo = (string)Session["correo"];
            correo = correo.Trim();
            if (correo == "")
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.correo = correo;
            }
            return View();
        }

        // POST: Cliente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,correo,nombre,apPaterno,apMaterno,fecha_nacimiento,sexo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();

                Session["idCliente"] = cliente.id;
                Session["name"] = cliente.nombre;
                Session["usr"] = cliente.nombre;

                if (Session["CrearOrden"] != null)
                {
                    if (Session["CrearOrden"].Equals("pend"))
                    {
                        Session["idCliente"] = cliente.id;
                        Session["name"] = cliente.nombre;
                        Session["usr"] = cliente.nombre;
                        return RedirectToAction("Create", "Tarjeta");
                    }
                    else
                    {
                        //TODO: REDIRECCIONAR A DETALLES
                        return RedirectToAction("index", "Home");

                    }
                }
                else
                {
                    //TODO: REDIRECCIONAR A DETALLES
                    return RedirectToAction("index", "Home");
                }

            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,correo,nombre,apPaterno,apMaterno,fecha_nacimiento,sexo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                Session["idCliente"] = cliente.id;
                Session["name"] = cliente.nombre;
                Session["usr"] = cliente.nombre;
                return RedirectToAction("MisDatos");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("MisDatos");
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

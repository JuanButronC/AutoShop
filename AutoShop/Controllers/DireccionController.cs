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
    public class DireccionController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Direccion
        public ActionResult Index()
        {
            var direccion = db.Direccion.Include(d => d.Cliente);
            return View(direccion.ToList());
        }
        // GET: Direccion
        public ActionResult MisDirecciones()
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
            var direccion = db.Direccion.Where(d => d.id_cliente_fk == id).Include(d => d.Cliente);
            Session["idCliente"] = id;
            return View(direccion.ToList());
        }


        // GET: Direccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccion.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // GET: Direccion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Direccion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_cliente_fk,estado,municipio,colonia,calle,numExterior,codigo_postal,tel")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                direccion.id_cliente_fk = (int)Session["idCliente"];
                db.Direccion.Add(direccion);
                db.SaveChanges();

                if (Session["CrearOrden"] != null)
                {
                    if (Session["CrearOrden"].Equals("pend"))
                    {
                        Session["CrearOrden"] = "";
                        return RedirectToAction("CrearOrden", "Pago");
                    }
                    else
                    {
                        //TODO: REDIRECCIONAR A DETALLES
                        return RedirectToAction("MisDirecciones");

                    }
                }
                else
                {
                    //TODO:REDIRECCIONAR A DETALLES
                    return RedirectToAction("MisDirecciones");
                }

            }

            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", direccion.id_cliente_fk);
            return View(direccion);
        }

        // GET: Direccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccion.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", direccion.id_cliente_fk);
            return View(direccion);
        }

        // POST: Direccion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_cliente_fk,estado,municipio,colonia,calle,numExterior,codigo_postal,tel")] Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(direccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MisDirecciones");
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", direccion.id_cliente_fk);
            return View(direccion);
        }

        // GET: Direccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Direccion direccion = db.Direccion.Find(id);
            if (direccion == null)
            {
                return HttpNotFound();
            }
            return View(direccion);
        }

        // POST: Direccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Direccion direccion = db.Direccion.Find(id);
            db.Direccion.Remove(direccion);
            db.SaveChanges();
            return RedirectToAction("MisDirecciones");
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

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
    public class EnvioController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Envio
        public ActionResult Index()
        {
            var envio = db.Envio.Where(e => e.fecha_envio == null).OrderBy(e => e.Orden.fecha_creacion).Include(e => e.Cliente).Include(e => e.Direccion).Include(e => e.Orden).Include(e => e.Paqueteria);
            return View(envio.ToList());
        }


        public ActionResult Index1()
        {
            var envio = db.Envio.Where(e => e.fecha_entrega == null && e.fecha_envio != null).OrderBy(e => e.fecha_envio).Include(e => e.Cliente).Include(e => e.Direccion).Include(e => e.Orden).Include(e => e.Paqueteria);
            return View(envio.ToList());
        }

        //Método para redireccionar a home
        public ActionResult Home()
        {
            List<Envio> query = (from i in db.Envio
                                 select i).ToList();

            var conteoTotalPaqs = query.Count();

            var enviosHoy = 0;
            var enviosSemana = 0;
            var enviosAnio = 0;
            var fechahoy = DateTime.Today.Date;
            var fechasemana = DateTime.Today.Date.AddDays(-7);
            var fechaanio = DateTime.Today.Date.AddYears(-1);
            foreach (Envio listado in (List<Envio>)query)
            {
                DateTime date = Convert.ToDateTime(listado.fecha_envio);
                if (date <= fechahoy)
                {
                    enviosHoy++;
                }
                if (date <= fechasemana)
                {
                    enviosSemana++;
                }
                if (date <= fechaanio)
                {
                    enviosAnio++;
                }
            }

            ViewBag.enviosHoy = enviosHoy;
            ViewBag.enviosSemana = enviosSemana;
            ViewBag.enviosAnio = enviosAnio;
            ViewBag.fechahoy = fechahoy;
            ViewBag.fechasemana = fechasemana;
            ViewBag.fechaanio = fechaanio;
            ViewBag.listadoPedidos = query;
            return View();
        }

        // GET: Envio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envio.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // GET: Envio/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "contrasenia");
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado");
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion");
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre");
            return View();
        }

        // POST: Envio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_orden_fk,id_paq_fk,id_cliente_fk,id_direccion_fk,fecha_envio,fecha_entrega,numero_guia")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Envio.Add(envio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "contrasenia", envio.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", envio.id_direccion_fk);
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion", envio.id_orden_fk);
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre", envio.id_paq_fk);
            return View(envio);
        }
        // GET: Envio/Edit/5
        public ActionResult Edit1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envio.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", envio.id_direccion_fk);
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion", envio.id_orden_fk);
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre", envio.id_paq_fk);
            return View(envio);
        }


        // GET: Envio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envio.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", envio.id_direccion_fk);
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion", envio.id_orden_fk);
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre", envio.id_paq_fk);
            return View(envio);
        }

        // POST: Envio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1([Bind(Include = "id,id_orden_fk,id_paq_fk,id_cliente_fk,id_direccion_fk,fecha_envio,fecha_entrega,numero_guia")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(envio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "contrasenia", envio.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", envio.id_direccion_fk);
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion", envio.id_orden_fk);
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre", envio.id_paq_fk);
            return View(envio);
        }


        // POST: Envio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_orden_fk,id_paq_fk,id_cliente_fk,id_direccion_fk,fecha_envio,fecha_entrega,numero_guia")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(envio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "contrasenia", envio.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", envio.id_direccion_fk);
            ViewBag.id_orden_fk = new SelectList(db.Orden, "id", "numero_confirmacion", envio.id_orden_fk);
            ViewBag.id_paq_fk = new SelectList(db.Paqueteria, "id", "nombre", envio.id_paq_fk);
            return View(envio);
        }

        // GET: Envio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envio.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // POST: Envio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Envio envio = db.Envio.Find(id);
            db.Envio.Remove(envio);
            db.SaveChanges();
            return RedirectToAction("Index");
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

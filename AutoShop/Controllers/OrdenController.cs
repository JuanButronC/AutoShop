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
    public class OrdenController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Orden
        public ActionResult Index()
        {
            var orden = db.Orden.Include(o => o.Cliente).Include(o => o.Direccion).Include(o => o.Tarjeta);
            return View(orden.ToList());
        }

        // GET: Orden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Orden/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo");
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado");
            ViewBag.id_tarjeta_fk = new SelectList(db.Tarjeta, "id", "nombre");
            return View();
        }

        // POST: Orden/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_cliente_fk,id_direccion_fk,id_tarjeta_fk,fecha_creacion,numero_confirmacion,total")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Orden.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", orden.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", orden.id_direccion_fk);
            ViewBag.id_tarjeta_fk = new SelectList(db.Tarjeta, "id", "nombre", orden.id_tarjeta_fk);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", orden.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", orden.id_direccion_fk);
            ViewBag.id_tarjeta_fk = new SelectList(db.Tarjeta, "id", "nombre", orden.id_tarjeta_fk);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_cliente_fk,id_direccion_fk,id_tarjeta_fk,fecha_creacion,numero_confirmacion,total")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente_fk = new SelectList(db.Cliente, "id", "correo", orden.id_cliente_fk);
            ViewBag.id_direccion_fk = new SelectList(db.Direccion, "id", "estado", orden.id_direccion_fk);
            ViewBag.id_tarjeta_fk = new SelectList(db.Tarjeta, "id", "nombre", orden.id_tarjeta_fk);
            return View(orden);
        }

        // GET: Orden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden orden = db.Orden.Find(id);
            db.Orden.Remove(orden);
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

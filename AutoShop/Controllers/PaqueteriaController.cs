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
    public class PaqueteriaController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Paqueteria
        public ActionResult Index()
        {
            return View(db.Paqueteria.ToList());
        }

        // GET: Paqueteria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paqueteria paqueteria = db.Paqueteria.Find(id);
            if (paqueteria == null)
            {
                return HttpNotFound();
            }
            return View(paqueteria);
        }

        // GET: Paqueteria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Paqueteria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,tipo,transporte")] Paqueteria paqueteria, HttpPostedFileBase imagen)
        {


            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    paqueteria.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                db.Paqueteria.Add(paqueteria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paqueteria);

        }

        // GET: Paqueteria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paqueteria paqueteria = db.Paqueteria.Find(id);
            if (paqueteria == null)
            {
                return HttpNotFound();
            }
            return View(paqueteria);
        }

        // POST: Paqueteria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,tipo,transporte")] Paqueteria paqueteria, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    paqueteria.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(paqueteria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paqueteria);
        }

        // GET: Paqueteria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paqueteria paqueteria = db.Paqueteria.Find(id);
            if (paqueteria == null)
            {
                return HttpNotFound();
            }
            return View(paqueteria);
        }

        // POST: Paqueteria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paqueteria paqueteria = db.Paqueteria.Find(id);
            db.Paqueteria.Remove(paqueteria);
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

        //Método para nueva vista de home

        public ActionResult Home()
        {
            List<Paqueteria> query = (from i in db.Paqueteria
                                      select i).ToList();

            var conteoTotalPaqs = query.Count();

            var conteoTerrestres = 0;
            var conteoAereos = 0;
            var conteoInternacional = 0;
            var conteoNacional = 0;
            foreach (Paqueteria listado in (List<Paqueteria>)query)
            {
                if (listado.tipo.Equals("Internacional"))
                {
                    conteoInternacional++;
                }
                else
                {
                    conteoNacional++;
                }
                if (listado.transporte.Equals("Terrestre"))
                {
                    conteoTerrestres++;
                }
                else
                {
                    conteoAereos++;
                }
            }

            ViewBag.conteoTotalPaqs = conteoTotalPaqs;
            ViewBag.conteoTerrestres = conteoTerrestres;
            ViewBag.conteoAereos = conteoAereos;
            ViewBag.conteoInternacional = conteoInternacional;
            ViewBag.conteoNacional = conteoNacional;
            ViewBag.listadoPaqueterias = query;
            return View();
        }


        //metodo para obtener imagen
        public ActionResult getImg(int id)
        {
            Paqueteria imgPaq = (from i in db.Paqueteria
                                 where i.id == id
                                 select i).ToList().FirstOrDefault();

            var fileToRetrieve = imgPaq.imagen;
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

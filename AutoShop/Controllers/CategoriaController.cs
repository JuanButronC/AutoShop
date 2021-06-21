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
    public class CategoriaController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Categoria
        public ActionResult Index()
        {
            return View(db.Categoria.ToList());
        }

        // GET: Categoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion")] Categoria categoria, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    categoria.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }

            if (ModelState.IsValid)
            {
                db.Categoria.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion")] Categoria categoria, HttpPostedFileBase imagen)
        {

            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    categoria.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                int id = categoria.id;
                var prod = db.Categoria.Find(id);
                //db.Entry(producto).State = EntityState.Modified;
                prod.nombre = categoria.nombre;
                prod.descripcion = categoria.descripcion;
                prod.imagen = categoria.imagen;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categoria.Find(id);
            db.Categoria.Remove(categoria);
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

        //metodo para obtener imagen
        public ActionResult getImg(int id)
        {
            Categoria imgPaq = (from i in db.Categoria
                                where i.id == id
                                select i).ToList().FirstOrDefault();

            var fileToRetrieve = imgPaq.imagen;
            return File(fileToRetrieve, "image/jpeg");
        }

        public ActionResult getExistenciasCat()
        {
            var producto = db.Categoria.Take(3).ToList();
            return View(producto);
        }
    }
}

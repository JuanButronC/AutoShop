using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    public class productoController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: producto
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Categoria);
            return View(producto.ToList());
        }

        // GET: producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: producto/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.Categoria, "id", "nombre");
            return View();
        }

        // POST: producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,id_categoria,descripcion,descuento,precio,cantidad")] Producto producto, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    producto.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                Producto prod = new Producto();
                prod.id = producto.id;
                prod.nombre = producto.nombre;
                prod.id_categoria = producto.id_categoria;
                prod.descripcion = producto.descripcion;
                prod.imagen = producto.imagen;
                prod.precio = producto.precio;
                prod.descuento = Descuento(producto.descuento);
                prod.cantidad = producto.cantidad;

                db.Producto.Add(prod);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.Categoria, "id", "nombre", producto.id_categoria);
            return View(producto);
        }

        public decimal Descuento(decimal? desc)
        {
            decimal descuento = 0;
            switch (desc)
            {
                case 10:
                    descuento = 10;
                    break;
                case 20:
                    descuento = 20;
                    break;
                case 30:
                    descuento = 30;
                    break;
                case 40:
                    descuento = 40;
                    break;
                case 50:
                    descuento = 50;
                    break;
                case 60:
                    descuento = 60;
                    break;
                case 70:
                    descuento = 70;
                    break;
                case 80:
                    descuento = 80;
                    break;
                case 90:
                    descuento = 90;
                    break;
                case 100:
                    descuento = 100;
                    break;
            }
            return descuento;
        }


        // GET: producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.Categoria, "id", "nombre", producto.id_categoria);
            return View(producto);
        }

        // POST: producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,id_categoria,descuento,descripcion,precio,cantidad")] Producto producto, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    producto.imagen = reader.ReadBytes(imagen.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                int id = producto.id;
                var prod = db.Producto.Find(id);
                decimal precio_ant = prod.precio;
                decimal precio_act = producto.precio;
                //db.Entry(producto).State = EntityState.Modified;
                prod.nombre = producto.nombre;
                prod.descripcion = producto.descripcion;
                prod.descuento = producto.descuento;
                prod.precio = producto.precio;
                prod.imagen = producto.imagen;
                prod.cantidad = producto.cantidad;
                prod.id_categoria = producto.id_categoria;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_categoria = new SelectList(db.Categoria, "id", "nombre", producto.id_categoria);
            return View(producto);
        }

        // GET: producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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
            Producto imgPaq = (from i in db.Producto
                               where i.id == id
                               select i).ToList().FirstOrDefault();

            var fileToRetrieve = imgPaq.imagen;
            return File(fileToRetrieve, "image/jpeg");
        }

        public ActionResult getProductos()
        {
            var producto = db.Producto.OrderByDescending(e => e.id)
                .Include(p => p.Categoria).Take(5).ToList();
            return View(producto);
        }

        public ActionResult getExistencias()
        {
            var producto = db.Producto.OrderBy(e => e.cantidad)
                .Include(p => p.Categoria).Take(5).ToList();
            return View(producto);
        }

        public ActionResult getProdMayor()
        {
            var producto = db.Producto.OrderByDescending(e => e.cantidad)
                .Include(p => p.Categoria).Take(1).ToList();
            return View(producto);
        }



    }
}

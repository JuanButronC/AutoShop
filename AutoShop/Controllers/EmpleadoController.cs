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
    public class EmpleadoController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Empleado
        public ActionResult Index()
        {
            var empleado = db.Empleado.Where(e => e.rol_fk != 3).Include(e => e.Roles);
            return View(empleado.ToList());
        }
        // GET: Empleado/Details/5
        public ActionResult DetailsPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }
        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            ViewBag.rol_fk = new SelectList(db.Roles, "id", "nombre");
            return View();
        }

        // POST: Empleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,contrasenia,correo,rol_fk")] Empleado empleado, HttpPostedFileBase imagen_fk)
        {
            if (imagen_fk != null && imagen_fk.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen_fk.InputStream))
                {
                    ImagenesEmpleado imEmp = new ImagenesEmpleado();
                    imEmp.imagen = reader.ReadBytes(imagen_fk.ContentLength);
                    db.ImagenesEmpleado.Add(imEmp);
                    empleado.imagen_fk = imEmp.id;
                    if (ModelState.IsValid)
                    {
                        db.Empleado.Add(empleado);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.rol_fk = new SelectList(db.Roles, "id", "nombre", empleado.rol_fk);
            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            ViewBag.rol_fk = new SelectList(db.Roles, "id", "nombre", empleado.rol_fk);
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,contrasenia,correo,rol_fk,imagen_fk")] Empleado empleado, HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(imagen.InputStream))
                {
                    ImagenesEmpleado imEmp = db.ImagenesEmpleado.Find(empleado.imagen_fk);
                    imEmp.imagen = reader.ReadBytes(imagen.ContentLength);
                    db.Entry(imEmp).State = EntityState.Modified;
                    db.SaveChanges();
                    if (ModelState.IsValid)
                    {
                        db.Entry(empleado).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.rol_fk = new SelectList(db.Roles, "id", "nombre", empleado.rol_fk);
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.Empleado.Find(id);
            ImagenesEmpleado imgEmpleado = db.ImagenesEmpleado.Find(empleado.imagen_fk);
            db.Empleado.Remove(empleado);
            db.ImagenesEmpleado.Remove(imgEmpleado);
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

        public ActionResult Imagen(int id)
        {
            ImagenesEmpleado img = (from i in db.ImagenesEmpleado
                                    where i.id == id
                                    select i).ToList().FirstOrDefault();

            var fileToRetrieve = img.imagen;
            return File(fileToRetrieve, "image/jpeg");
        }
    }
}
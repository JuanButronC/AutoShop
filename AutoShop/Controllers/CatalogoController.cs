using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;
using System.Data.Entity;

namespace AutoShop.Controllers
{
    public class CatalogoController : Controller
    {
        private contextAutoShop db = new contextAutoShop();

        // GET: Catalogo
        public ActionResult Index()
        {
            var producto = db.Producto.Include(p => p.Categoria).ToList();
            var categorias = db.Categoria.ToList();
            var descuentos = from p in db.Producto
                             where p.descuento > 0
                             select p.descuento;
            setPageViewBag(producto);
            ViewBag.categorias = categorias.ToList();
            ViewBag.descuentos = descuentos.Distinct().ToList();

            return View();
        }
        private void setPageViewBag(List<Producto> producto)
        {

            int countProd = producto.Count;
            double div = ((double)countProd / 9);
            int pageCount = (int)Math.Ceiling(div);

            ViewBag.selectedPage = 1;

            ViewBag.productos = producto.ToList();

            ViewBag.pageCount = pageCount;
        }


        public ActionResult getCategoria(int id)
        {

            var categoria = (from c in db.Categoria
                             where c.id == id
                             select c).ToList().FirstOrDefault();
            ViewBag.categoria = categoria;

            return View();
        }
        public ActionResult todosProductos()
        {

            var producto = db.Producto.ToList();
            setPageViewBag(producto);

            return View();
        }
        public ActionResult productosCategoria(int id)
        {

            var producto = (from p in db.Producto
                            where p.id_categoria == id
                            select p).ToList();
            setPageViewBag(producto);

            return View();
        }

        [HttpPost]
        public ActionResult BuscarProducto(string nombreProducto)
        {
            ViewBag.SearchKey = nombreProducto;

            var query = from st in db.Producto
                        where st.nombre.Contains(nombreProducto)
                        select st;
            var listProd = query.ToList();
            var categorias = db.Categoria.ToList();
            var descuentos = from p in db.Producto
                             where p.descuento > 0
                             select p.descuento;
            setPageViewBag(listProd);
            ViewBag.categorias = categorias.ToList();
            ViewBag.descuentos = descuentos.Distinct().ToList();
            return View();
        }

        public ActionResult ImagenProducto(int id)
        {
            Producto img = (from i in db.Producto
                            where i.id == id
                            select i).ToList().FirstOrDefault();

            var fileToRetrieve = img.imagen;
            return File(fileToRetrieve, "image/jpeg");
        }
        public ActionResult ImagenCategoria(int id)
        {
            Categoria img = (from i in db.Categoria
                             where i.id == id
                             select i).ToList().FirstOrDefault();

            var fileToRetrieve = img.imagen;
            return File(fileToRetrieve, "image/jpeg");
        }

        public ActionResult Producto(int id)
        {
            Producto producto = db.Producto.Find(id);
            ViewBag.producto = producto;
            var productosSimilares = (from p in db.Producto
                                      where p.id_categoria == producto.id_categoria && p.id != producto.id
                                      select p).ToList();
            int count = productosSimilares.Count;
            if (count >= 3)
            {
                ViewBag.productos = productosSimilares.GetRange(0, 3);
            }
            else
            {
                ViewBag.productos = productosSimilares;

            }
            var comentarios = (from p in db.Comentarios
                               where p.id_producto_fk == producto.id
                               select p).ToList();
            ViewBag.comentarios = comentarios;

            return View();
        }

        [HttpPost]
        public ActionResult Comentar(int id, string comentario)
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = User.Identity.Name;
                var cliente = (from c in db.Cliente
                               where c.correo == correo
                               select c.id).ToList().FirstOrDefault();
                Comentarios comentarios = new Comentarios();
                comentarios.id_cliente_fk = cliente;
                comentarios.id_producto_fk = id;
                comentarios.comentario = comentario;
                db.Comentarios.Add(comentarios);
                db.SaveChanges();
                return RedirectToAction("Producto", routeValues: new { id = id });
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoShop.Models;

namespace AutoShop.Models
{
    public class ProductoCarro
    {
        private contextAutoShop db = new contextAutoShop();
        private List<Producto> productos;

        public ProductoCarro()
        {
            productos = db.Producto.ToList();
        }

        public List<Producto> findAll()
        {
            return this.productos;
        }

        public Producto find(int id)
        {
            Producto pp = this.productos.Single(p => p.id.Equals(id));
            return pp;
        }
    }
}
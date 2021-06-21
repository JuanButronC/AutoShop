using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoShop.Models
{
    public class Item
    {
        public Producto Producto
        {
            get;
            set;
        }

        public int Cantidad
        {
            get;
            set;
        }
    }
}
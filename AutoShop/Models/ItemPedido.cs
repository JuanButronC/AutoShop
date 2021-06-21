using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoShop.Models
{
    public class ItemPedido
    {

        public int idOrd
        {
            get;
            set;
        }

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
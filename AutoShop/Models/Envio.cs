//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Envio
    {
        public int id { get; set; }
        public int id_orden_fk { get; set; }
        public int id_paq_fk { get; set; }
        public int id_cliente_fk { get; set; }
        public int id_direccion_fk { get; set; }
        public Nullable<System.DateTime> fecha_envio { get; set; }
        public Nullable<System.DateTime> fecha_entrega { get; set; }
        public string numero_guia { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Direccion Direccion { get; set; }
        public virtual Orden Orden { get; set; }
        public virtual Paqueteria Paqueteria { get; set; }
    }
}

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
    
    public partial class Tarjeta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tarjeta()
        {
            this.Orden = new HashSet<Orden>();
        }
    
        public int id { get; set; }
        public int id_cliente_fk { get; set; }
        public string nombre { get; set; }
        public string numero_cuenta { get; set; }
        public int mes_vencimiento { get; set; }
        public int anio_vencimiento { get; set; }
        public int cvv { get; set; }
        public int tipoTarj { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orden> Orden { get; set; }
    }
}

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
    using System.ComponentModel.DataAnnotations;

    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.Comentarios = new HashSet<Comentarios>();
            this.Orden_Producto = new HashSet<Orden_Producto>();
        }

        public int id { get; set; }
        [Required(ErrorMessage = "Favor de ingresar un nombre para el producto.")]
        [Display(Name = "Producto")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Favor de ingresar una categoría.")]
        [Display(Name = "Categoría")]
        public int id_categoria { get; set; }
        [Required(ErrorMessage = "Favor de ingresar una descripción.")]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Imagen")]
        public byte[] imagen { get; set; }
        [Required(ErrorMessage = "Favor de ingresar un precio.")]
        [Display(Name = "Precio")]
        [Range(1, 500, ErrorMessage = "Favor de ingresar un precio valido ($1.00 - $500.00)")]
        public decimal precio { get; set; }
        [Display(Name = "Descuento")]
        public Nullable<decimal> descuento { get; set; }
        [Required(ErrorMessage = "Favor de ingresar una cantidad.")]
        [Display(Name = "Cantidad")]
        [Range(1, 100, ErrorMessage = "Favor de ingresar una cantidad válida (1-100)")]
        public int cantidad { get; set; }

        public virtual Categoria Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comentarios> Comentarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orden_Producto> Orden_Producto { get; set; }
    }
}

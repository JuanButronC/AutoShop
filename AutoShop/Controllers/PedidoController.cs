using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        contextAutoShop db = new contextAutoShop();
        // GET: Pedido
        public ActionResult Index()
        {
            string correo = User.Identity.Name;
            Cliente cl = (from c in db.Cliente
                          where c.correo == correo
                          select c).ToList().FirstOrDefault();

            int id = cl.id;

            var query = from o in db.Orden
                        where o.id_cliente_fk == id
                        orderby o.fecha_creacion ascending
                        select o;
            List<Orden> ordenes = query.ToList();

            List<PedidoCliente> pedidos = new List<PedidoCliente>();
            PedidoCliente pedido;
            List<Orden_Producto> ordPed;
            List<ItemPedido> itemPed = new List<ItemPedido>();

            ItemPedido iPed;

            foreach (Orden o in ordenes)
            {
                pedido = new PedidoCliente();
                pedido.Orden = o;
                pedido.Fecha = o.fecha_creacion.ToShortDateString();
                var envio = (from e in db.Envio
                             where e.id_orden_fk == o.id
                             select e).ToList().FirstOrDefault();
                var entrega = (from e in db.Envio
                               where e.id_orden_fk == o.id
                               select e).ToList().FirstOrDefault();
                if (envio.fecha_envio.HasValue)
                {
                    pedido.envio = envio.fecha_envio.GetValueOrDefault().ToShortDateString();
                }
                else
                {
                    pedido.envio = "Próximamente";
                }
                if (entrega.fecha_entrega.HasValue)
                {
                    pedido.status = entrega.fecha_entrega.GetValueOrDefault().ToShortDateString();
                }
                else
                {
                    pedido.status = "Sin Entregar";
                }
                pedido.Total = o.total.ToString();
                pedidos.Add(pedido);
                ordPed = (from oP in db.Orden_Producto
                          where oP.id_orden_fk == o.id
                          select oP).ToList();
                pedido.ordenProductos = ordPed;
                foreach (Orden_Producto op in ordPed)
                {
                    iPed = new ItemPedido();
                    iPed.idOrd = op.id_orden_fk;
                    iPed.Producto = db.Producto.First(p => p.id == op.id_prod_fk);
                    iPed.Cantidad = op.cantidad;
                    itemPed.Add(iPed);
                }
            }
            Session["misPedidos"] = pedidos;
            Session["Pedido"] = itemPed;

            return View();
        }
    }
}
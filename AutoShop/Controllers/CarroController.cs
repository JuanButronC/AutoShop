using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoShop.Models;

namespace AutoShop.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agregar(int id, int cantidad)
        {
            ProductoCarro carro = new ProductoCarro();
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                Producto p = carro.find(id);
                string name = p.nombre;
                cart.Add(new Item { Producto = carro.find(id), Cantidad = cantidad });
                setItemCount(cart);
                Session["cart"] = cart;

            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Cantidad += cantidad;
                }
                else
                {
                    Producto P = carro.find(id);
                    string name = P.nombre;
                    cart.Add(new Item { Producto = carro.find(id), Cantidad = cantidad });
                }
                setItemCount(cart);
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }
        private int isExist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Producto.id.Equals(id))
                    return i;
            return -1;
        }



        public ActionResult SumarUno(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            if (index != -1)
            {

                cart[index].Cantidad++;
                if (cart[index].Producto.cantidad == cart[index].Cantidad)
                {
                    cart[index].Cantidad--;
                }
                setItemCount(cart);

            }
            return RedirectToAction("Index");
        }

        public ActionResult QuitarUno(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            if (index != -1)
            {
                cart[index].Cantidad--;
                if (cart[index].Cantidad == 0)
                {
                    cart.RemoveAt(index);
                }
                setItemCount(cart);
            }
            return RedirectToAction("Index");
        }

        private void setItemCount(List<Item> cart)
        {
            int cont = 0;
            foreach (Item itm in cart)
            {
                cont = cont + itm.Cantidad;
            }
            Session["itemTotal"] = cont;
        }

        public ActionResult Quitar(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            setItemCount(cart);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
    }
}
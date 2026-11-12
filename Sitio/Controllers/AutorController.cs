using EntidadesCompartidas;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio.Controllers
{
    public class AutorController : Controller
    {
        // GET: Autor
        public ActionResult Listado() //Listado Autores
        {
            List<Autor> listaA = new List<Autor>();

            try
            {
                //Traemos el listado desde la LogicaPublicacion

                listaA = LogicaAutor.ListadoAutores();

                if (listaA.Count == 0)
                    ViewBag.Mensaje = "No hay autores para mostrar";

            }
            catch (Exception ex)
            {

                ViewBag.Mensaje = ex.Message;
            }
            return View(listaA);
        }

        //operacion auxiliar
        private Autor BuscarAutor(string pCodigoInternoAutor)
        {
            Autor objeto = LogicaAutor.BuscarAutor(pCodigoInternoAutor);

            if (objeto is null)
                throw new Exception("No existe un autor con ese código interno");

            return objeto;


        }


        //AGREGAR AUTOR
        [HttpGet]
        public ActionResult Agregar() => View();


        [HttpPost]
        public ActionResult Agregar(Autor pAutor)
        {
            try
            {
                pAutor.Validar();
                LogicaAutor.Agregar(pAutor);

                //Si todo sale bien, redirigimos al listado:
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(pAutor);
            }
        }

        public ActionResult Consultar(string pCodigoInternoAutor = null)
        {
            try
            {
                //Si la URL esta incompleta el parametro queda con su valor por defecto
                if (pCodigoInternoAutor == null)
                    return RedirectToAction("Listado");

                return View(BuscarAutor(pCodigoInternoAutor));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Autor());
            }
        }


        //Modificar autor
        [HttpGet]
        public ActionResult Modificar(string pCodigoInternoAutor = null) 
        {
            try
            {
                if (pCodigoInternoAutor == null || pCodigoInternoAutor == "") 
                    return RedirectToAction("Listado");
                //Si el parametro es correcto, buscamos la publicacion digital y la pasamos a la vista
                return View(BuscarAutor(pCodigoInternoAutor));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Autor());
            }
        }

        [HttpPost]
        public ActionResult Modificar(Autor pAutor)
        {
            try
            {
                pAutor.Validar(); 
                LogicaAutor.Modificar(pAutor);
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(pAutor);
            }
        }


        //eliminar autor
        public ActionResult Eliminar(string pCodigoInternoAutor = null)
        {
            try
            {
                if (pCodigoInternoAutor == null || pCodigoInternoAutor == "") 
                    return RedirectToAction("Listado");
                
                return View(BuscarAutor(pCodigoInternoAutor));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Autor()); 
            }
        }

        [HttpPost]
        public ActionResult Eliminar(Autor pAutor)
        {
            try
            {
                LogicaAutor.Eliminar(pAutor);
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(pAutor);
            }
        }
    }
}
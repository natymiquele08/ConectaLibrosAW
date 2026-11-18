using EntidadesCompartidas;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio.Controllers
{
    public class LibreriaController : Controller
    {
        // CRUD LIBRERIA: Listado
        public ActionResult Listado()
        {
            
            List<Libreria> listaL = new List<Libreria>();

            try
            {

                listaL = LogicaLibreria.ListadoLibreria();

                if (listaL.Count == 0)
                    ViewBag.Mensaje = "No hay librerias para mostrar";

            }
            catch (Exception ex)
            {

                ViewBag.Mensaje = ex.Message;
            }
            return View(listaL);
        }

        //operacion auxiliar
        private Libreria BuscarLibreria(string pRut)
        {
            Libreria objeto = LogicaLibreria.BuscarLibreria(pRut);

            if (objeto is null)
                throw new Exception("No existe ua libreria con ese RUT");

            return objeto;

        }

        //Agregar libreria
        [HttpGet]
        public ActionResult Agregar() => View();


        [HttpPost]
        public ActionResult Agregar(Libreria pLibreria)
        {
            try
            {
                pLibreria.Validar();
                LogicaLibreria.Agregar(pLibreria);

                //Si todo sale bien, redirigimos al listado:
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(pLibreria);
            }
        }

        //Consultar
        public ActionResult Consultar(string pRut = null)
        {
            try
            {
                //Si la URL esta incompleta el parametro queda con su valor por defecto
                if (pRut == null)
                    return RedirectToAction("Listado");

                return View(BuscarLibreria(pRut));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Libreria());
            }
        }


        //Modificar

        [HttpGet]
        public ActionResult Modificar(string pRut = null)
        {
            try
            {
                if (pRut == null || pRut == "")
                    return RedirectToAction("Listado");
                
                return View(BuscarLibreria(pRut));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Libreria());
            }
        }

        [HttpPost]
        public ActionResult Modificar(Libreria pLibreria)
        {
            try
            {
                pLibreria.Validar();
                LogicaLibreria.Modificar(pLibreria);
                return RedirectToAction("Listado");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(pLibreria);
            }
        }
    }
}

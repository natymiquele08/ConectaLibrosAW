using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //Pagina DEFAULT
        public ActionResult Index()
        {
            return View();
        }
    }
}
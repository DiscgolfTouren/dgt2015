using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGTMVC4.Controllers
{
    public class TournamentsController : Controller
    {
        public ActionResult Tournament(int id = -1)
        {
            ViewBag.Message = string.Format("Tävling: {0}", id);

            return View();
        }

        public ActionResult Registration(int id = -1)
        {
            ViewBag.Message = string.Format("Registrering för tävling: {0}", id);

            return View();
        }

        public ActionResult Result(int id = -1)
        {
            ViewBag.Message = string.Format("Resultat för tävling: {0}", id);

            return View();
        }

        public ActionResult Standings()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DGTMVC4.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userId, string password)
        {
            if(IsValid(userId, password))
            {
                FormsAuthentication.SetAuthCookie(userId, false);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Inloggningen misslyckades ...");
            }
            return View(userId);
        }

        private bool IsValid(string userId, string password)
        {
            return true;
        }

    }
}

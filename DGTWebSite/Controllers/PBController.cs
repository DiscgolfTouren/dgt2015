using System.Web.Mvc;
using DGTWebSite.Models;

namespace DGTWebSite.Controllers
{
    public class PBController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome(string name, int number = 1)
        {
            return View(new PB() {Name = "Namnet är " + name, Number = number});
        }
    }
}
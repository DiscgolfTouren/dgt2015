using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGTMVC4.Models;
using DGTMVC4.NHibernate;
using DGTMVC4.NHibernate.Models;
using NHibernate.Linq;

namespace DGTMVC4.Controllers
{
    public class DiscgolftourenController : Controller
    {
        //
        // GET: /Discgolftouren/
        public ActionResult Index()
        {
            var vm = new CompetitionsViewModel();
            var competitions = new List<Competition>();
            using (var session = NHibernateFactory.OpenSession())
            {
                competitions = session.Query<Competition>().Where(c => c.Date.Year == DateTime.Now.Year).OrderBy(c => c.Date).ToList();
            }
            foreach (var competition in competitions)
            {
                var cp = new CompetitionDTO()
                    {
                        Id = competition.Id,
                        Name = competition.Name,
                        Date = competition.Date
                    };
                vm.Competitions.Add(cp);
            }

            return View(vm);
        }

        public ActionResult Om()
        {
            return View();
        }

        public ActionResult Tournaments()
        {

            return View();
        }

        public ActionResult Kontakt()
        {
            return View();
        }

        public ActionResult Hur()
        {
            return View();
        }
	}
}
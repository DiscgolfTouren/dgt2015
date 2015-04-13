using DGTMVC4.Models;
using DGTMVC4.NHibernate;
using DGTMVC4.NHibernate.Models;
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

        public ActionResult Registration(RegistrationViewModel vm, string registreraAnmalan, string kontrolleraPDGA)
        {
            if (kontrolleraPDGA != null)
            {
                // kontrollera PDGA nummer
                if (!string.IsNullOrEmpty(vm.PDGANummer))
                {
                    // om spelare redan reggad
                    //     om Ok
                    //        visa spelaren
                    // annars kontrollera med pdga
                    //     om Ok
                    //        regga spelaren
                    //        visa spelaren

                    // PDGA kontroll
                    vm.PDGANummer = vm.PDGANummer.Trim();
                    int pdgaNummer = 0;
                    if (int.TryParse(vm.PDGANummer, out pdgaNummer))
                    {
                        var player = HamtaPlayer(vm.PDGANummer);
                        PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(vm.PDGANummer);
                        int playerRating = 0;
                        int.TryParse(pdgaPlayer.rating, out playerRating);
                        if (pdgaPlayer != null && pdgaPlayer.pdga_number != null && pdgaPlayer.membership_status == "current" && playerRating >= 930)
                        {
                            vm.SpelareOk = true;
                            vm.Fornamn = pdgaPlayer.first_name;
                            vm.Efternamn = pdgaPlayer.last_name;
                            // registrera spelaren
                        }
                        else
                        {
                            if (playerRating < 930 && pdgaPlayer.membership_status == "current")
                            {
                                vm.Meddelande = "Din rating måste vara 930 eller högre";
                            }
                            else
                            {
                                vm.Meddelande = string.Format("Angivet PDGAnummer: {0} är inte Ok!", vm.PDGANummer);
                            }
                        }
                    }
                    else
                    {
                        vm.Meddelande = string.Format("Angivet PDGAnummer: {0} är inte numeriskt!", vm.PDGANummer);
                    }
                }
            }
            else if (registreraAnmalan != null)
            {
                // registrera anmälan
                vm.SpelareAnmald = true;
            }

            return View(vm);
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

        public Player HamtaPlayer(string PDGANummer)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var player = session.QueryOver<Player>().Where(p => p.PdgaNumber == PDGANummer); 

            }

            return null;
        }
    }
}

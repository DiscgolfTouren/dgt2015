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
                    vm.PDGANummer = vm.PDGANummer.Trim();
                    int pdgaNummer = 0;
                    if (int.TryParse(vm.PDGANummer, out pdgaNummer))
                    {
                        // kontrollera om redan registrerad i systemet
                        var player = HamtaPlayer(vm.PDGANummer);
                        if (player != null && player.RatingDate.Year == DateTime.Now.Year)
                        {
                            vm.SpelareOk = true;
                            vm.Fornamn = player.FirstName;
                            vm.Efternamn = player.LastName;
                        }
                        else // om inte i systemet kontrollera med PDGA
                        {
                            PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(vm.PDGANummer);
                            int playerRating = 0;
                            int.TryParse(pdgaPlayer.rating, out playerRating);
                            if (pdgaPlayer != null && pdgaPlayer.pdga_number != null && pdgaPlayer.membership_status == "current" && playerRating >= 930)
                            {
                                vm.SpelareOk = true;
                                vm.Fornamn = pdgaPlayer.first_name;
                                vm.Efternamn = pdgaPlayer.last_name;
                                var playerModel = new Player();
                                playerModel.FirstName = pdgaPlayer.first_name;
                                playerModel.LastName = pdgaPlayer.last_name;
                                playerModel.PdgaNumber = pdgaPlayer.pdga_number;
                                playerModel.Rating = pdgaPlayer.rating;
                                playerModel.RatingDate = DateTime.Now;

                                SparaPlayer(playerModel);
                                vm.SpelareId = playerModel.Id;
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
                // todo när det finns mer än en tävling så har den förmodligen id != 1
                vm.TavlingsId = 1;
                RegistreraAnmalan(vm);
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

        private void SparaPlayer(Player player)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(player);
                    transaction.Commit();
                }
            }
        }

        public Player HamtaPlayer(string PDGANummer)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var players = session.QueryOver<Player>().Where(p => p.PdgaNumber == PDGANummer).List<Player>();
                if (players.Count > 0)
                {
                    return players[0];
                }
            }

            return null;
        }

        public Competition HamtaCompetition(int id)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var competition = session.Get<Competition>(id);
                

                return competition;
            }
        }

        private void RegistreraAnmalan(RegistrationViewModel vm)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var playerstatusModel = new PlayerStatus();
                    var player = HamtaPlayer(vm.PDGANummer);
                    var competition = HamtaCompetition(vm.TavlingsId);
                    if(player != null && competition != null)
                    {
                        playerstatusModel.Competition = competition;
                        playerstatusModel.Player = player;
                        playerstatusModel.Status = NHibernate.Enums.PlayerCompetitionStatus.Registered;
                        session.SaveOrUpdate(playerstatusModel);
                        transaction.Commit();
                    }
                }
            }
        }


    }
}

using DGTMVC4.Models;
using DGTMVC4.NHibernate;
using DGTMVC4.NHibernate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using NHibernate.Linq;

namespace DGTMVC4.Controllers
{
    public class TournamentsController : Controller
    {
        public ActionResult Tournament(int id = -1)
        {
            var vm = new CompetitionsViewModel();

            if(id != -1)
            {
                // hämta data för tävling
                using(var session = NHibernateFactory.OpenSession())
                {
                    var competition = session.Get<Competition>(id);
                    if(competition != null)
                    {
                        vm.Competition = new CompetitionDTO()
                        {
                            Id = competition.Id,
                            Name = competition.Name,
                            Date = competition.Date,
                            PGDAWebPage = competition.PdgaWebPage,
                            Description = competition.Description
                        };

                        var players = session.Query<PlayerStatus>().Where(p => p.Competition.Id == competition.Id);

                        foreach (var player in players.Where(p => p.Status == NHibernate.Enums.PlayerCompetitionStatus.Payed))
                        {
                            vm.Competition.Players.Add(new PlayerDTO()
                            {
                                Namn = string.Format("{0} {1}", player.Player.FirstName, player.Player.LastName),
                                PDGA = player.Player.PdgaNumber,
                                Rating = player.Player.Rating
                            });
                        }

                        foreach (var player in players.Where(p => p.Status == NHibernate.Enums.PlayerCompetitionStatus.Registered))
                        {
                            vm.Competition.RegisteredPlayers.Add(new PlayerDTO()
                            {
                                Namn = string.Format("{0} {1}", player.Player.FirstName, player.Player.LastName),
                                PDGA = player.Player.PdgaNumber,
                                Rating = player.Player.Rating
                            });
                        }

                        foreach (var player in players.Where(p => p.Status == NHibernate.Enums.PlayerCompetitionStatus.Waiting))
                        {
                            vm.Competition.WaitingPlayers.Add(new PlayerDTO()
                            {
                                Namn = string.Format("{0} {1}", player.Player.FirstName, player.Player.LastName),
                                PDGA = player.Player.PdgaNumber,
                                Rating = player.Player.Rating
                            });
                        }
                    }
                }
            }
            else
            {
                // hämta data för tävlingar
                var competitions = new List<Competition>();
                using (var session = NHibernateFactory.OpenSession())
                {
                    competitions = session.Query<Competition>().ToList();
                }
                
                foreach (var competition in competitions)
                {
                    var cp = new CompetitionDTO()
                    {
                        Id = competition.Id,
                        Name = competition.Name,
                        Date = competition.Date,
                    };
                    vm.Competitions.Add(cp);
                }
            }

            return View(vm);
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
                            vm.SpelareId = player.Id;
                            vm.Fornamn = player.FirstName;
                            vm.Efternamn = player.LastName;




                            // todo tävling hårdkodad
                            vm.SpelareRegistrerad = SpelareRegistrerad(2, player.Id);





                        }
                        else // om inte i systemet kontrollera med PDGA
                        {
                            PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(vm.PDGANummer, WebConfigurationManager.AppSettings["PDGAUsername"], WebConfigurationManager.AppSettings["PDGAPassword"]);
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




                // todo när det finns mer än en tävling så har den förmodligen id != 2
                vm.TavlingsId = 2;



                RegistreraAnmalan(vm);
                vm.SpelareAnmald = true;
            }

            vm.Competitions = GetPlayerCompetitions(vm.SpelareId);

            return View(vm);
        }

        private List<PlayerCompetitionDTO> GetPlayerCompetitions(int spelareId)
        {
            var competitions = new List<Competition>();
            using (var session = NHibernateFactory.OpenSession())
            {
                competitions = session.Query<Competition>().Where(c => c.Date.Year == DateTime.Now.Year).OrderBy(c => c.Date).ToList();
            }

            var playerCompetitons = new List<PlayerCompetitionDTO>();
            foreach (var competition in competitions)
            {
                var pc = new PlayerCompetitionDTO()
                {
                    CompetitionId = competition.Id,
                    CompetitionName = competition.Name,
                    CompetitionDate = competition.Date
                };
                var player = competition.Players.Where(p => p.Player.Id == spelareId).ToList<PlayerStatus>();
                if (player.Count() > 0)
                {
                    pc.PlayerStatus = player.First().Status;
                }

                playerCompetitons.Add(pc);
            }

            return playerCompetitons;
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

        private bool SpelareRegistrerad(int competitionId, int playerId)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var playerStatuses = session.QueryOver<PlayerStatus>().Where(ps => ps.Player.Id == playerId && ps.Competition.Id == competitionId).List<PlayerStatus>();
                return playerStatuses.Count > 0;
            }
        }

        public List<PlayerStatus> HamtaCompetitionPlayers(int id)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var competition = session.Get<Competition>(id);

                return competition != null ? competition.Players.ToList() : new List<PlayerStatus>();
            }
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

                    var player = HamtaPlayer(vm.PDGANummer);
                    var competition = HamtaCompetition(vm.TavlingsId);

                    if(player != null && competition != null)
                    {
                        PlayerStatus playerStatus;
                        var playerStatuses = session.QueryOver<PlayerStatus>().Where(ps => ps.Player.Id == player.Id && ps.Competition.Id == competition.Id).List<PlayerStatus>();
                        if(playerStatuses.Count > 0)
                        {
                            playerStatus = playerStatuses[0];
                        }
                        else
                        {
                            playerStatus = new PlayerStatus();
                        }

                        playerStatus.Competition = competition;
                        playerStatus.Player = player;
                        playerStatus.Status = NHibernate.Enums.PlayerCompetitionStatus.Registered;
                        session.SaveOrUpdate(playerStatus);
                        transaction.Commit();
                    }
                }
            }
        }


    }
}

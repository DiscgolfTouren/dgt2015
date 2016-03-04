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
        public ActionResult Tournament(CompetitionsViewModel vm, int id = -1)
        {
            vm.YearsList = new List<SelectListItem>();

            AddYear(vm.YearsList, "2015", vm.SelectedYear);
            AddYear(vm.YearsList, "2016", vm.SelectedYear);


            if (id != -1)
            {
                // hämta data för tävling
                using (var session = NHibernateFactory.OpenSession())
                {
                    var competition = session.Get<Competition>(id);
                    if (competition != null)
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
                    competitions = session.Query<Competition>().Where(c => c.Date.Year == int.Parse(vm.SelectedYear)).OrderBy(c => c.Date).ToList();
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

        private void AddYear(List<SelectListItem> list, string year, string selectedYear)
        {
            list.Add(new SelectListItem { Text = year, Value = year, Selected = (year==selectedYear) });
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
                        if (player != null)
                        {
                            vm.SpelareId = player.Id;
                            vm.Fornamn = player.FirstName;
                            vm.Efternamn = player.LastName;

                            if(player.RatingDate.Year != DateTime.Now.Year)
                            {
                                PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(vm.PDGANummer, WebConfigurationManager.AppSettings["PDGAUsername"], WebConfigurationManager.AppSettings["PDGAPassword"]);
                                if(pdgaPlayer != null && pdgaPlayer.pdga_number != null && pdgaPlayer.membership_status == "current")
                                {
                                    UpdateRating(vm.PDGANummer, pdgaPlayer.rating, DateTime.Now);
                                    player.Rating = pdgaPlayer.rating;
                                }
                            }

                            int playerRating = 0;
                            int.TryParse(player.Rating, out playerRating);
                            if(playerRating >= 920)
                            {
                                vm.SpelareOk = true;
                            }

                        }
                        else // om inte i systemet kontrollera med PDGA
                        {
                            PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(vm.PDGANummer, WebConfigurationManager.AppSettings["PDGAUsername"], WebConfigurationManager.AppSettings["PDGAPassword"]);
                            int playerRating = 0;
                            int.TryParse(pdgaPlayer.rating, out playerRating);
                            if (pdgaPlayer != null && pdgaPlayer.pdga_number != null && pdgaPlayer.membership_status == "current" && playerRating >= 920)
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
                                if (playerRating < 920 && pdgaPlayer.membership_status == "current")
                                {
                                    vm.Meddelande = "Din rating måste vara 920 eller högre";
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

                if (registreraAnmalan.Contains("1"))
                {
                    vm.TavlingsId = vm.Competitions[0].CompetitionId;
                }
                else if (registreraAnmalan.Contains("2"))
                {
                    vm.TavlingsId = vm.Competitions[1].CompetitionId;
                }
                else if (registreraAnmalan.Contains("3"))
                {
                    vm.TavlingsId = vm.Competitions[2].CompetitionId;
                }
                else if (registreraAnmalan.Contains("4"))
                {
                    vm.TavlingsId = vm.Competitions[3].CompetitionId;
                }
                else if (registreraAnmalan.Contains("5"))
                {
                    vm.TavlingsId = vm.Competitions[4].CompetitionId;
                }

                RegistreraAnmalan(vm);
                //vm.SpelareAnmald = true;
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
                    CompetitionDate = competition.Date,
                    Greenfee = competition.Greenfee,
                    RegistrationOpens = competition.RegistrationOpens
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
            var vm = new StandingsViewModel();

            // hämta data för tabell
            var standings = new List<Standing>();
            using (var session = NHibernateFactory.OpenSession())
            {
                standings = session.Query<Standing>().ToList();
            }

            foreach (var standing in standings)
            {
                var s = new StandingPlayerDTO()
                {
                    Placering = standing.Place,
                    Namn = string.Format("{0} {1}", standing.Player.FirstName, standing.Player.LastName),
                    PDGA = standing.Player.PdgaNumber,
                    TotalPoang = standing.TotalPoints,
                    DGT1Placering = standing.DGT1Place,
                    DGT1Poang = standing.DGT1Points,
                    DGT2Placering = standing.DGT2Place,
                    DGT2Poang = standing.DGT2Points,
                    DGT3Placering = standing.DGT3Place,
                    DGT3Poang = standing.DGT3Points,
                    DGT4Placering = standing.DGT4Place,
                    DGT4Poang = standing.DGT4Points,
                    DGT5Placering = standing.DGT5Place,
                    DGT5Poang = standing.DGT5Points
                };
                vm.Standings.Add(s);
            }


            // Debug - test ...
            //for (int i = 0; i < 30; i++)
            //{
            //    vm.Standings.Add(new StandingPlayerDTO()
            //    {
            //        Placering = 30-i,
            //        Namn = "Peter Bygde",
            //        PDGA = "8558",
            //        TotalPoang = i + 23,
            //        DGT1Placering = 23,
            //        DGT1Poang = 12,
            //        DGT2Placering = 23,
            //        DGT2Poang = 12,
            //        DGT3Placering = 23,
            //        DGT3Poang = 12,
            //        DGT4Placering = 23,
            //        DGT4Poang = 12,
            //        DGT5Placering = 23,
            //        DGT5Poang = 12
            //    });
            //}

            return View(vm);
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

        private void UpdateRating(string PDGANummer, string rating, DateTime ratingDate)
        {
            var player = HamtaPlayer(PDGANummer);
            if(player != null)
            {
                player.Rating = rating;
                player.RatingDate = ratingDate;
                SparaPlayer(player);
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

        public Player HamtaPlayer(long spelareId)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var players = session.QueryOver<Player>().Where(p => p.Id == spelareId).List<Player>();
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

                    var player = HamtaPlayer(vm.SpelareId);
                    var competition = HamtaCompetition(vm.TavlingsId);

                    if (player != null && competition != null)
                    {
                        PlayerStatus playerStatus;
                        var playerStatuses = session.QueryOver<PlayerStatus>().Where(ps => ps.Player.Id == player.Id && ps.Competition.Id == competition.Id).List<PlayerStatus>();
                        if (playerStatuses.Count > 0)
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

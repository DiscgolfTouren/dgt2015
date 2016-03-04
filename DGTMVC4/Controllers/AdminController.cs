using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGTMVC4.Enum;
using DGTMVC4.Models;
using FluentNHibernate.Conventions;
using DGTMVC4.NHibernate;
using DGTMVC4.NHibernate.Models;
using NHibernate.Linq;
using System.Web.Configuration;

namespace DGTMVC4.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Results(AdminResultsViewModel vm, string tolkaInput, string uppdateraTournament, string uppdateraStandings)
        {
            // Init
            if (vm.Competitions == null)
            {
                var competitons = GetCompetitions();

                var ddlCompetitions = new List<SelectListItem>();
                ddlCompetitions.Add(new SelectListItem { Text = "Välj en tävling", Value = "-1", Selected = true });

                foreach (var c in competitons)
                {
                    ddlCompetitions.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                }

                vm.Competitions = ddlCompetitions;
            }

            if (tolkaInput != null)
            {
                var results = TolkaInput(vm.Indata);
                vm.AdminResults = results;
                vm.Utdata = string.Format("{0}", results.Count);
            }

            if (uppdateraStandings != null)
            {
                UpdateStandings();
            }

            if (uppdateraTournament != null)
            {
                if (vm.tournamentId != -1)
                {
                    var results = TolkaInput(vm.Indata);

                    if (ResultsOk(results))
                    {
                        DeleteCompetitionResults(vm.tournamentId);

                        var competition = GetCompetition(vm.tournamentId);

                        if (competition != null)
                        {
                            foreach (var r in results)
                            {
                                AddResult(competition, r);
                            }
                        }
                    }
                    else
                    {
                        vm.ResultsOk = false;
                    }
                }
            }

            return View(vm);
        }

        private void UpdateStandings()
        {
            int year = DateTime.Now.Year;

            DeleteStandings(year);

            using (var session = NHibernateFactory.OpenSession())
            {

                var results = session.Query<Result>().Where(r => r.Competition.Date.Year == year).OrderBy(r => r.Competition).OrderBy(r => r.Place);
                var competitions = session.Query<Competition>().Where(c => c.Date.Year == year);

                // hämta lista med tävlingar
                // för varje tävling
                //   för varje spelare
                //     räkna ut poäng
                foreach (var competition in competitions)
                {
                    var numberOfPlayers = results.Count(r => r.Competition == competition);
                    var competitionResults = results.Where(r => r.Competition == competition);
                    foreach (var cr in competitionResults)
                    {
                        var samePlace = competitionResults.Where(c => c.Place == cr.Place).Count();
                        cr.Points = CalculatePoints(numberOfPlayers, cr.Place, samePlace);
                    }
                }

                var standings = session.Query<Standing>();
                foreach (var result in results)
                {
                    var standing = standings.FirstOrDefault(s => s.Player == result.Player);
                    if (standing == null)
                    {
                        // new
                        standing = new Standing
                        {
                            Year = year,
                            Player = result.Player
                        };

                        UpdatePlacePoints(standing, result, competitions);
                        session.Save(standing);
                    }
                    else
                    {
                        // update
                        UpdatePlacePoints(standing, result, competitions);
                    }
                }

                foreach (var standing in standings)
                {
                    CalculateTotalPoints(standing);
                }

                // sortera efter division och points desc
                // en räknare
                // om nästa har ny division sätt index till 1
                // annars om nästa har lägre poäng
                //    sätt räknaren lika med index
                //    öka räknaren med 1
                // öka index med 1
                var standingsAddPlace = standings.ToList().OrderByDescending(s => s.TotalPoints);

                int i = 0;
                int index = 0;
                double points = 501.0;
                foreach(var standing in standingsAddPlace)
                {
                    i++;
                    if(Math.Round(standing.TotalPoints, 2) < Math.Round(points, 2))
                    {
                        index = i;
                        points = standing.TotalPoints;
                    }

                    standing.Place = index;
                }

                session.Flush();

            }


        }

        private void CalculateTotalPoints(Standing standing)
        {
            List<double> points = new List<double>();
            if (standing.DGT1Points != null) { points.Add(standing.DGT1Points); }
            if (standing.DGT2Points != null) { points.Add(standing.DGT2Points); }
            if (standing.DGT3Points != null) { points.Add(standing.DGT3Points); }
            if (standing.DGT4Points != null) { points.Add(standing.DGT4Points); }
            if (standing.DGT5Points != null) { points.Add(standing.DGT5Points); }

            double min = 0.0;

            if(points.Count > 4)
            {
                min = 100.0;
                foreach(var p in points)
                {
                    if(p < min)
                    {
                        min = p;
                    }
                }
            }

            double totalPoints = points.Sum() - min;

            standing.TotalPoints = totalPoints;
        }

        private void UpdatePlacePoints(Standing standing, Result result, IQueryable<Competition> competitions)
        {
            int i = 0;
            foreach (var c in competitions.OrderBy(c => c.Date))
            {
                i++;
                if (c == result.Competition)
                {
                    break;
                }
            }

            switch (i)
            {
                case 1:
                    standing.DGT1Place = result.Place;
                    standing.DGT1Points = result.Points;
                    break;
                case 2:
                    standing.DGT2Place = result.Place;
                    standing.DGT2Points = result.Points;
                    break;
                case 3:
                    standing.DGT3Place = result.Place;
                    standing.DGT3Points = result.Points;
                    break;
                case 4:
                    standing.DGT4Place = result.Place;
                    standing.DGT4Points = result.Points;
                    break;
                case 5:
                    standing.DGT5Place = result.Place;
                    standing.DGT5Points = result.Points;
                    break;
            }
        }

        private double CalculatePoints(int numberOfPlayers, int place, int samePlace)
        {
            double points = 0.0;
            for (int i = place; i < place + samePlace; i++)
            {
                points += CalculatePoints(numberOfPlayers, i);
            }

            return points / samePlace;
        }

        private double CalculatePoints(int numberOfPlayers, int place)
        {
            if (place < 11)
            {
                return 100 + 1 - place;
            }
            else
            {
                double diff = 91.0 / (numberOfPlayers - 9);
                double points = 91.0 - (place - 10) * diff;
                return points;
            }
        }

        private void CalculatePlace()
        {
            throw new NotImplementedException();
        }

        private void CalculateTotalPoints()
        {
            throw new NotImplementedException();
        }

        private List<CompetitionDTO> GetCompetitions()
        {
            var dtos = new List<CompetitionDTO>();
            using (var session = NHibernateFactory.OpenSession())
            {
                var competitions = session.Query<Competition>().Where(c => c.Date.Year == DateTime.Now.Year).ToList();
                foreach (var c in competitions)
                {
                    dtos.Add(new CompetitionDTO()
                    {
                        Date = c.Date,
                        Description = c.Description,
                        Id = c.Id,
                        Name = c.Name,
                        PGDAWebPage = c.PdgaWebPage
                    });
                }
            }

            return dtos;
        }

        private Competition GetCompetition(int competitionId)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                return session.Query<Competition>().FirstOrDefault(c => c.Id == competitionId);
            }
        }

        private void AddResult(Competition competition, AdminResult adminResult)
        {
            // find player
            using (var session = NHibernateFactory.OpenSession())
            {
                var player = session.Query<Player>().FirstOrDefault(p => p.PdgaNumber == adminResult.PDGA);
                if (player == null)
                {
                    // hämta från PDGA
                    PDGASaker.PDGAPlayer pdgaPlayer = PDGASaker.PDGARESTApi.GetMemberInfo(adminResult.PDGA, WebConfigurationManager.AppSettings["PDGAUsername"], WebConfigurationManager.AppSettings["PDGAPassword"]);
                    if (pdgaPlayer != null)
                    {
                        var newPlayer = new Player();
                        newPlayer.PdgaNumber = pdgaPlayer.pdga_number;
                        newPlayer.LastName = pdgaPlayer.last_name;
                        newPlayer.FirstName = pdgaPlayer.first_name;
                        newPlayer.Rating = pdgaPlayer.rating;
                        newPlayer.RatingDate = DateTime.Now;

                        session.Save(newPlayer);

                        player = newPlayer;
                    }

                }

                if (player == null)
                {
                    return;
                }

                var result = new Result();
                result.Competition = competition;
                result.Player = player;
                result.R1 = int.Parse(adminResult.R1);
                result.R2 = int.Parse(adminResult.R2);
                result.Total = int.Parse(adminResult.Total);
                result.Place = int.Parse(adminResult.Place);

                session.Save(result);
            }
        }

        private bool ResultsOk(List<AdminResult> results)
        {
            return results.All(r => r.Status == AdminResultStatus.Ok);
        }

        private void DeleteCompetitionResults(int competitionId)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var results = session.QueryOver<Result>().Where(r => r.Competition.Id == competitionId).List<Result>();
                foreach (var r in results)
                {
                    session.Delete(r);
                }

                session.Flush();
            }
        }

        private void DeleteStandings(int year)
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                var standings = session.QueryOver<Standing>().Where(s => s.Year == year).List<Standing>();
                foreach (var s in standings)
                {
                    session.Delete(s);
                }

                session.Flush();
            }
        }

        private List<AdminResult> TolkaInput(string input)
        {
            var results = new List<AdminResult>();

            var reader = new StringReader(input);
            var firstLine = reader.ReadLine();

            int place = FindIndexOf(firstLine, "place");
            int name = FindIndexOf(firstLine, "Name");
            int pdga = FindIndexOf(firstLine, "PDGA#");
            int r1 = FindIndexOf(firstLine, "Rd1");
            int r2 = FindIndexOf(firstLine, "Rd2");
            int total = FindIndexOf(firstLine, "Total");

            var line = reader.ReadLine();
            while (line != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var result = new AdminResult
                    {
                        Place = GetValue(line, place),
                        Name = GetValue(line, name),
                        PDGA = GetValue(line, pdga),
                        R1 = GetValue(line, r1),
                        R2 = GetValue(line, r2),
                        Total = GetValue(line, total)
                    };

                    if (result.Total.ToUpper() == "DNF")
                    {
                        result.Total = "-1";
                    }

                    if (string.IsNullOrEmpty(result.R1))
                    {
                        result.R1 = "0";
                    }

                    if (string.IsNullOrEmpty(result.R2))
                    {
                        result.R2 = "0";
                    }

                    results.Add(result);
                }

                line = reader.ReadLine();
            }

            return results;
        }

        private string GetValue(string line, int index)
        {
            if (index == -1)
            {
                return "";
            }

            var values = line.Split('\t');

            if (index >= values.Length)
            {
                return "";
            }

            return values[index];
        }

        private int FindIndexOf(string firstLine, string text)
        {
            var kollumner = firstLine.Split('\t');

            for (int i = 0; i < kollumner.Length; i++)
            {
                if (kollumner[i].Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

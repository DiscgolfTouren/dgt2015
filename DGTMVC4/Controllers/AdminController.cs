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

        public ActionResult Results(AdminResultsViewModel vm, string tolkaInput, string uppdateraTournament)
        {
            // Init
            if(vm.Competitions == null)
            {
                var competitons = GetCompetitions();

                var ddlCompetitions = new List<SelectListItem>();
                ddlCompetitions.Add(new SelectListItem { Text = "Välj en tävling", Value = "-1", Selected = true });

                foreach(var c in competitons)
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

        private List<CompetitionDTO> GetCompetitions()
        {
            var dtos = new List<CompetitionDTO>();
            using (var session = NHibernateFactory.OpenSession())
            {
                var competitions = session.Query<Competition>().Where(c => c.Date.Year == DateTime.Now.Year).ToList();
                foreach(var c in competitions)
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

                if(player == null)
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

                    if(result.Total.ToUpper() == "DNF")
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

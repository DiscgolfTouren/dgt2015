using System.Collections.Generic;
using DGTMVC4.NHibernate.Models;
using System.Web.Mvc;
using System;

namespace DGTMVC4.Models
{
    public class CompetitionsViewModel
    {
        public CompetitionsViewModel()
        {
            Competitions = new List<CompetitionDTO>();
            SelectedYear = DateTime.Now.Year.ToString();
        }

        public List<CompetitionDTO> Competitions { get; set; }
        public CompetitionDTO Competition { get; set; }
        public List<SelectListItem> YearsList {get; set;}
        public string SelectedYear {get; set;}
        public bool ShowCompetition 
        {
            get { return Competition != null; }
        }
    }
}
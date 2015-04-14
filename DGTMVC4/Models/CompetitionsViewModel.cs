using System.Collections.Generic;
using DGTMVC4.NHibernate.Models;

namespace DGTMVC4.Models
{
    public class CompetitionsViewModel
    {
        public CompetitionsViewModel()
        {
            Competitions = new List<CompetitionDTO>();
        }

        public List<CompetitionDTO> Competitions { get; set; } 
    }
}
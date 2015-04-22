using DGTMVC4.NHibernate.Enums;
using DGTMVC4.NHibernate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Models
{
    public class PlayerCompetitionDTO
    {
        public int CompetitionId { get; set; }
        public string CompetitionName { get; set; }
        public DateTime CompetitionDate { get; set; }
        public PlayerCompetitionStatus? PlayerStatus { get; set; }
    }
}
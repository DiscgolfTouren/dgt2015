using System;
using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Competition
    {
        public string Name { get; set; }
        public IDictionary<Player, PlayerCompetitionStatus> Players { get; set; }
        public DateTime Date { get; set; }
        public Courseconfiguration Configuration { get; set; }
    }
}
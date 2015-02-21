using System;
using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Competition
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IDictionary<Player, PlayerCompetitionStatus> Players { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Courseconfiguration Configuration { get; set; }
    }
}
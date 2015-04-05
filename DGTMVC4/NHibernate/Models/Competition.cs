using System;
using System.Collections.Generic;
using DGTMVC4.NHibernate.Enums;

namespace DGTMVC4.NHibernate.Models
{
    public class Competition
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IDictionary<Player, PlayerCompetitionStatus> Players { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual IList<Round> Rounds { get; set; }
        public virtual string PdgaWebPage { get; set; }
        public virtual string Description { get; set; }
    }
}
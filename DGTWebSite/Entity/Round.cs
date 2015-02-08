using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Round
    {
        public Competition Competition { get; set; }
        public Courseconfiguration Courseconfig { get; set; }
        public int RoundNumber { get; set; }
        public IDictionary<Player, int> PlayerResults { get; set; }
    }
}
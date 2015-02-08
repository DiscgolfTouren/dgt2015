using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Round
    {
        public Competition Competition { get; set; }
        public Courseconfiguration Courseconfig { get; set; }
        public int RoundNumber { get; set; }
        public IList<PlayerResult> PlayerResults { get; set; }
    }
}
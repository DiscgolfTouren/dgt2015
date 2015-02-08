using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class PlayerResult
    {
        public Player Player { get; set; }
        public IList<HoleScore> Scores { get; set; }
        public int Penalties { get; set; }
        public int Place { get; set; }
    }
}
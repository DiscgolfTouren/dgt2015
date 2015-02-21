using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Round
    {
        public virtual int Id { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual Courseconfiguration Courseconfig { get; set; }
        public virtual int RoundNumber { get; set; }
        public virtual IList<PlayerResult> PlayerResults { get; set; }
    }
}
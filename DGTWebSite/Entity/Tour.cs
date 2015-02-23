using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Tour
    {
        public virtual int Id { get; set; }
        public virtual string Year { get; set; }
        public virtual IList<Competition> Competitions { get; set; }
    }
}
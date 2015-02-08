using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Tour
    {
        public string Year { get; set; }
        public IList<Competition> Competitions { get; set; }
    }
}
using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Courseconfiguration
    {
        public string CourseName { get; set; }
        public IList<Hole> Holes { get; set; }
    }
}
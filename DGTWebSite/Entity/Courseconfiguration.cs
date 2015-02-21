using System.Collections.Generic;

namespace DGTWebSite.Entity
{
    public class Courseconfiguration
    {
        public virtual int Id { get; set; }
        public virtual string CourseName { get; set; }
        public virtual IList<Hole> Holes { get; set; }
    }
}
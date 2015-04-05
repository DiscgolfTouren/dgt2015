using System.Collections.Generic;

namespace DGTMVC4.NHibernate.Models
{
    public class Tour
    {
        public virtual int Id { get; set; }
        public virtual string Year { get; set; }
        public virtual IList<Competition> Competitions { get; set; }
        public virtual string Description { get; set; }
    }
}
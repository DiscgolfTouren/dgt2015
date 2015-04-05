using System.Collections.Generic;

namespace DGTMVC4.NHibernate.Models
{
    public class Courseconfiguration
    {
        public virtual int Id { get; set; }
        public virtual string CourseName { get; set; }
        public virtual IList<Hole> Holes { get; set; }
        public virtual string Description { get; set; }
        public virtual string CourseMap { get; set; }
    }
}
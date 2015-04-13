using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class CourseconfigurationMap : ClassMap<Courseconfiguration>
    {
         public CourseconfigurationMap()
         {
             Id(c => c.Id);
             Map(c => c.CourseName);
             HasMany(c => c.Holes)
                 .Inverse()
                 .Cascade.All();
             Map(c => c.Description);
             Map(c => c.CourseMap);
         }
    }
}
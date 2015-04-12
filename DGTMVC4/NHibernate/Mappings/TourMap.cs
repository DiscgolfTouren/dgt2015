using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class TourMap : ClassMap<Tour>
    {
         public TourMap()
         {
             Id(x => x.Id);
             Map(x => x.Year);
             HasMany(x => x.Competitions)
                 .Inverse()
                 .Cascade.All();
             Map(x => x.Description);
         }
    }
}
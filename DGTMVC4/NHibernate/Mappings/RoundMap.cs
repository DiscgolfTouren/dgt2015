using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class RoundMap : ClassMap<Round>
    {
         public RoundMap()
         {
             Id(x => x.Id);
             References(x => x.Competition);
             References(x => x.Courseconfig);
             Map(x => x.RoundNumber);
             HasMany(x => x.PlayerResults)
                 .Inverse()
                 .Cascade.All();
         }
    }
}
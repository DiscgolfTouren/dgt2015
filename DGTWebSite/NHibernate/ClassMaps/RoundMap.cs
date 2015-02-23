using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class RoundMap : ClassMap<Round>
    {
         public RoundMap()
         {
             Id(x => x.Id);
             References(x => x.Competition).Cascade.All();
             References(x => x.Courseconfig).Cascade.All();
             Map(x => x.RoundNumber);
             HasMany(x => x.PlayerResults);
         }
    }
}
using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class PlayerResultMap : ClassMap<PlayerResult>
    {
         public PlayerResultMap()
         {
             Id(x => x.Id);
             References(x => x.Player).Cascade.All();
             HasMany(x => x.Scores);
             Map(x => x.Penalties);
             Map(x => x.Place);
         } 
    }
}
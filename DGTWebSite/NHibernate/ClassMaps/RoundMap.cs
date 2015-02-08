using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class RoundMap : ClassMap<Round>
    {
         public RoundMap()
         {
             Map(r => r.Competition);
             Map(r => r.Courseconfig);
             Map(r => r.PlayerResults);
             Map(r => r.RoundNumber);
         }
    }
}
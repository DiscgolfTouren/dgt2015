using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class PlayerMap : ClassMap<Player>
    {
         public PlayerMap()
         {
             Id(p => p.Id);
             Map(p => p.FirstName);
             Map(p => p.LastName);
             Map(p => p.PdgaNumber).Not.Nullable();
             Map(p => p.Email).Not.Nullable();
             Map(p => p.Phone);
         }
    }
}
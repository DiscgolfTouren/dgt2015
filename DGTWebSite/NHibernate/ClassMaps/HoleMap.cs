using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class HoleMap : ClassMap<Hole>
    {
         public HoleMap()
         {
             Id(h => h.Id);
             Map(h => h.Number);
             Map(h => h.Par);
             Map(h => h.Elevation);
             Map(h => h.Description);
         }
    }
}
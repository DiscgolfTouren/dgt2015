using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class HoleMap : ClassMap<Hole>
    {
         public HoleMap()
         {
             Map(h => h.Number);
             Map(h => h.Par);
             Map(h => h.Elevation);
             Map(h => h.Description);
         }
    }
}
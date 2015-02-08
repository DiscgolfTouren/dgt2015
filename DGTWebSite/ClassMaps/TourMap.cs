using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.ClassMaps
{
    public class TourMap : ClassMap<Tour>
    {
        public TourMap()
        {
            Map(t => t.Competitions);
            Map(t => t.Year);
        }
    }
}
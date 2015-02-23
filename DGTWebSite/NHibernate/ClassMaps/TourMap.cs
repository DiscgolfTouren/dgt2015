using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class TourMap : ClassMap<Tour>
    {
        public TourMap()
        {
            Id(x => x.Id);
            Map(x => x.Year);
            HasMany(x => x.Competitions);
        }
    }
}
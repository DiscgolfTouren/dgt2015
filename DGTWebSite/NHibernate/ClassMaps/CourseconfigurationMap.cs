using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class CourseconfigurationMap : ClassMap<Courseconfiguration>
    {
         public CourseconfigurationMap()
         {
             Id(c => c.Id);
             Map(c => c.CourseName);
             HasMany(c => c.Holes);
         }
    }
}
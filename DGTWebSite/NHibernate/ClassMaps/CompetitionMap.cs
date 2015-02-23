using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class CompetitionMap : ClassMap<Competition>
    {
        public CompetitionMap()
        {
            Id(c => c.Id);
            Map(c => c.Name);
            HasMany(c => c.Players);
            Map(c => c.Date);
            Map(c => c.Configuration);
        }
    }
}
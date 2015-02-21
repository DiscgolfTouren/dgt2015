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
            References(c => c.Players).Cascade.All();
            Map(c => c.Configuration);
            Map(c => c.Date);
        }
    }
}
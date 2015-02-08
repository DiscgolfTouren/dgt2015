using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class CompetitionMap : ClassMap<Competition>
    {
        public CompetitionMap()
        {
            Map(c => c.Name);
            Map(c => c.Players);
            Map(c => c.Configuration);
            Map(c => c.Date);
        }
    }
}
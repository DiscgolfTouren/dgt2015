using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class ResultMap : ClassMap<Result>
    {
        public ResultMap()
        {
            Id(c => c.Id);
            References(c => c.Competition).Not.LazyLoad();
            References(c => c.Player).Not.LazyLoad();
            Map(c => c.Place);
            Map(c => c.R1);
            Map(c => c.R2);
            Map(c => c.Total);
            Map(c => c.Points);
        }
    }
}
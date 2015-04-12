using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class PlayerResultMap : ClassMap<PlayerResult>
    {
        public PlayerResultMap()
        {
            Id(x => x.Id);
            References(x => x.Player);
            HasMany(x => x.Scores)
                .Inverse()
                .Cascade.All();
            Map(x => x.Penalties);
            Map(x => x.Place);
            References(x => x.Round);
        }
         
    }
}
using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class CompetitionMap : ClassMap<Competition>
    {
         public CompetitionMap()
         {
             Id(x => x.Id);
             Map(x => x.Name);
             HasMany(x => x.Players)
                 .Inverse()
                 .Cascade.All();
             Map(x => x.Date);
             HasMany(x => x.Rounds)
                 .Inverse()
                 .Cascade.All();
             Map(x => x.PdgaWebPage);
             Map(x => x.Description);
             References(x => x.Tour);
             References(x => x.Round);
         }
    }
}
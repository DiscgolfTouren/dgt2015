using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class PlayerStatusMap : ClassMap<PlayerStatus>
    {
         public PlayerStatusMap()
         {
             Id(x => x.Id);
             References(x => x.Player);
             Map(x => x.Status);
             References(x => x.Competition);
         }
    }
}
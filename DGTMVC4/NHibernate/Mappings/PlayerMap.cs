using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class PlayerMap : ClassMap<Player>
    {
         public PlayerMap()
         {
             Id(p => p.Id);
             Map(p => p.FirstName);
             Map(p => p.LastName);
             Map(p => p.Email);
             Map(p => p.Phone);
             Map(p => p.PdgaNumber);
             Map(p => p.Rating);
             Map(p => p.RatingDate);
             Map(p => p.WildCardYear);
         }
    }
}
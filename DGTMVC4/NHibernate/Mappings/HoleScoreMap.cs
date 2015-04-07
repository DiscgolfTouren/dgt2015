using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class HoleScoreMap : ClassMap<HoleScore>
    {
        public HoleScoreMap()
        {
            Id(h => h.Id);
            Map(h => h.HoleNumber);
            Map(h => h.Score);
        }
         
    }
}
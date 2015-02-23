using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.NHibernate.ClassMaps
{
    public class HoleScoreMap : ClassMap<HoleScore>
    {
        public HoleScoreMap()
        {
            Id(x => x.Id);
            References(x => x.Hole).Cascade.All();
            Map(x => x.Score);
        }
         
    }
}
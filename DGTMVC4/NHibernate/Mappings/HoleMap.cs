using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;

namespace DGTMVC4.NHibernate.Mappings
{
    public class HoleMap : ClassMap<Hole>
    {
        public HoleMap()
        {
            Id(h => h.Id);
            Map(h => h.Number);
            Map(h => h.Par);
            Map(h => h.Elevation);
            Map(h => h.Length);
            Map(h => h.Description);
        }
         
    }
}
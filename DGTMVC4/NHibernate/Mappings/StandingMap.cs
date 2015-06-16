using DGTMVC4.NHibernate.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.NHibernate.Mappings
{
    public class StandingMap : ClassMap<Standing>
    {
        public StandingMap()
        {
            Id(c => c.Id);
            Map(c => c.Place);
            References(c => c.Player).Not.LazyLoad();
            Map(c => c.Year);
            Map(c => c.TotalPoints);
            Map(c => c.DGT1Place);
            Map(c => c.DGT1Points);
            Map(c => c.DGT2Place);
            Map(c => c.DGT2Points);
            Map(c => c.DGT3Place);
            Map(c => c.DGT3Points);
            Map(c => c.DGT4Place);
            Map(c => c.DGT4Points);
            Map(c => c.DGT5Place);
            Map(c => c.DGT5Points);
        }

    }
}
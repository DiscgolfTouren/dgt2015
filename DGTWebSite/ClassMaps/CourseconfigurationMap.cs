﻿using DGTWebSite.Entity;
using FluentNHibernate.Mapping;

namespace DGTWebSite.ClassMaps
{
    public class CourseconfigurationMap : ClassMap<Courseconfiguration>
    {
         public CourseconfigurationMap()
         {
             Map(c => c.CourseName);
             Map(c => c.Holes);
         }
    }
}
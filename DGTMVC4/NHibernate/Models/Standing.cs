﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.NHibernate.Models
{
    public class Standing
    {
        public virtual int Id { get; set; }
        public virtual int Place { get; set; }
        public virtual Player Player { get; set; }
        public virtual int TotalPoints { get; set; }
        public virtual int DGT1Place { get; set; }
        public virtual int DGT1Points { get; set; }
        public virtual int DGT2Place { get; set; }
        public virtual int DGT2Points { get; set; }
        public virtual int DGT3Place { get; set; }
        public virtual int DGT3Points { get; set; }
        public virtual int DGT4Place { get; set; }
        public virtual int DGT4Points { get; set; }
        public virtual int DGT5Place { get; set; }
        public virtual int DGT5Points { get; set; }
        public virtual int Year { get; set; }

    }
}
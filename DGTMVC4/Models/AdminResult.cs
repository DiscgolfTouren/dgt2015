using DGTMVC4.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Models
{
    public class AdminResult
    {
        public string Place { get; set; }
        public string PDGA { get; set; }
        public string Name { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }
        public string Total { get; set; }
        public AdminResultStatus Status { get; set; }
    }
}
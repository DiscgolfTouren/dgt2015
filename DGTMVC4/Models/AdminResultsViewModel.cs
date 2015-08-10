using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Models
{
    public class AdminResultsViewModel
    {
        public string Indata { get; set; }
        public string  Utdata { get; set; }
        public List<AdminResult> AdminResults { get; set; }
    }
}
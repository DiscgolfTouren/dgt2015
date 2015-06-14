using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Models
{
    public class StandingsViewModel
    {
        public StandingsViewModel()
        {
            Standings = new List<StandingPlayerDTO>();
        }

        public List<StandingPlayerDTO>  Standings { get; set; }
    }

    public class StandingPlayerDTO
    {
        public int Placering { get; set; }
        public string Namn { get; set; }
        public string PDGA { get; set; }
        public int TotalPoang { get; set; }
        public int DGT1Placering { get; set; }
        public double DGT1Poang { get; set; }
        public int DGT2Placering { get; set; }
        public double DGT2Poang { get; set; }
        public int DGT3Placering { get; set; }
        public double DGT3Poang { get; set; }
        public int DGT4Placering { get; set; }
        public double DGT4Poang { get; set; }
        public int DGT5Placering { get; set; }
        public double DGT5Poang { get; set; }
    }
    
}
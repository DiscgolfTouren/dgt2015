using System;
using System.Collections.Generic;
using System.Globalization;
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

        public List<StandingPlayerDTO> Standings { get; set; }
    }

    public class StandingPlayerDTO
    {
        public int Placering { get; set; }
        public string Namn { get; set; }
        public string PDGA { get; set; }
        public double TotalPoang { get; set; }
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

        public string TotalPoangText { get { return CheckDoubleZero(TotalPoang); } }
        public string DGT1PlaceringText { get { return CheckZero(DGT1Placering);}}
        public string DGT1PoangText { get{ return CheckDoubleZero(DGT1Poang);}}
        public string DGT2PlaceringText { get { return CheckZero(DGT2Placering); } }
        public string DGT2PoangText { get { return CheckDoubleZero(DGT2Poang); } }
        public string DGT3PlaceringText { get { return CheckZero(DGT3Placering); } }
        public string DGT3PoangText { get { return CheckDoubleZero(DGT3Poang); } }
        public string DGT4PlaceringText { get { return CheckZero(DGT4Placering); } }
        public string DGT4PoangText { get { return CheckDoubleZero(DGT4Poang); } }
        public string DGT5PlaceringText { get { return CheckZero(DGT5Placering); } }
        public string DGT5PoangText { get { return CheckDoubleZero(DGT5Poang); } }

        private string CheckZero(int value)
        {
            return value == 0 ? "" : value.ToString();
        }

        private string CheckDoubleZero(double value)
        {
            var valueTruncated = Math.Truncate(value * 100 / 100);
            CultureInfo ci = new CultureInfo("sv-SE");
            return value == 0 ? "" : string.Format(ci, "{0:0.00}", value);
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGTMVC4.Models
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel()
        {
        }

        public string PDGANummer { get; set; }
        public string Fornamn { get; set; }
        public string Efternamn { get; set; }

        public string EPost { get; set; }

        public int TavlingsId { get; set; }
        public int SpelareId { get; set; }
        public bool SpelareOk { get; set; }
        public bool SpelareAnmald { get; set; }
        public bool SpelareRegistrerad { get; set; }

        public string Meddelande { get; set; }

        public List<PlayerCompetitionDTO> Competitions { get; set; }
        //public List<CompetitionDTO> Competitions { get; set; }
    }
}
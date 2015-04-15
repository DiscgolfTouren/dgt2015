using System;
using System.Collections.Generic;

namespace DGTMVC4.Models
{
    public class CompetitionDTO
    {
        public CompetitionDTO()
        {
            Players = new List<PlayerDTO>();
            RegisteredPlayers = new List<PlayerDTO>();
            WaitingPlayers = new List<PlayerDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string PGDAWebPage { get; set; }
        public string Description { get; set; }
        public List<PlayerDTO> Players { get; set; }
        public List<PlayerDTO> RegisteredPlayers { get; set; }
        public List<PlayerDTO> WaitingPlayers { get; set; }
    }
}
﻿using System.Collections.Generic;

namespace DGTMVC4.NHibernate.Models
{
    public class Round
    {
        public virtual int Id { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual Courseconfiguration Courseconfig { get; set; }
        public virtual int RoundNumber { get; set; }
        public virtual IList<PlayerResult> PlayerResults { get; set; }

        public Round()
        {
            PlayerResults = new List<PlayerResult>();
        }

        public virtual void AddPlayerResult(PlayerResult playerResult)
        {
            playerResult.Round = this;
            PlayerResults.Add(playerResult);
        }
    }
}
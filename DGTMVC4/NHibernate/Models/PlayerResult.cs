using System.Collections.Generic;

namespace DGTMVC4.NHibernate.Models
{
    public class PlayerResult
    {
        public virtual int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual IList<HoleScore> Scores { get; set; }
        public virtual int Penalties { get; set; }
        public virtual int Place { get; set; }

        public virtual Round Round { get; set; }

        public PlayerResult()
        {
            Scores = new List<HoleScore>();
        }

        public virtual void AddHoleScore(HoleScore holeScore)
        {
            holeScore.PlayerResult = this;
            Scores.Add(holeScore);
        }
    }
}
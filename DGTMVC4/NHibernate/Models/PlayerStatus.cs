using DGTMVC4.NHibernate.Enums;

namespace DGTMVC4.NHibernate.Models
{
    public class PlayerStatus
    {
        public virtual int Id { get; protected set; }
        public virtual Player Player { get; set; }
        public virtual PlayerCompetitionStatus Status { get; set; }
        public virtual Competition Competition { get; set; }
    }
}
using System;

namespace DGTMVC4.NHibernate.Models
{
    public class Player
    {
        public virtual int Id { get; protected set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PdgaNumber { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Rating { get; set; }
        public virtual DateTime RatingDate { get; set; }
    }
}
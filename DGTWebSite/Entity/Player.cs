namespace DGTWebSite.Entity
{
    public class Player
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PdgaNumber { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
    }
}
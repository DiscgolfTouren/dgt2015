namespace DGTWebSite.Entity
{
    public class Hole
    {
        public virtual int Id { get; set; }
        public virtual int Number { get; set; }
        public virtual int Par { get; set; }
        public virtual int Elevation { get; set; }
        public virtual string Description { get; set; }
    }
}
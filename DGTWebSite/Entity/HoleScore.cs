namespace DGTWebSite.Entity
{
    public class HoleScore
    {
        public virtual int Id { get; set; }
        public virtual Hole Hole { get; set; }
        public virtual int Score { get; set; }
    }
}
namespace DGTMVC4.NHibernate.Models
{
    public class Result
    {
        public virtual int Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual int Place { get; set; }
        public virtual int R1 { get; set; }
        public virtual int R2 { get; set; }
    }
}
namespace trackingAPI.Models
{
    public class Team
    {

        //public Team()
        //{
        //    this.Matches = new HashSet<Match>();
        //}
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Match> Matches { get; set;}
    }
}

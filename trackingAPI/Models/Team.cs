using System.ComponentModel;

namespace trackingAPI.Models;

public class Team
{

    //public Team()
    //{
    //    this.Matches = new HashSet<Match>();
    //}
    public int Id { get; set; }
    
    public string Name { get; set; }

    [DefaultValue("true")]
    public bool? IsAvailable { get; set; }

    public ICollection<GameMatch> Matches { get; set;}
}

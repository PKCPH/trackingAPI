using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models;

public class Team
{

    //public Team()
    //{
    //    this.Matches = new HashSet<Match>();
    //}

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    [DefaultValue("true")]
    public bool? IsAvailable { get; set; }

    public ICollection<GameMatch> Matches { get; set;}
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace trackingAPI.Models
{
    public class Player
    {
        public Player() 
        {
            this.Teams = new HashSet<PlayerTeam>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public ICollection <PlayerTeam> Teams { get; set; }
    }
}
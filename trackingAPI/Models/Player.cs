using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace trackingAPI.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<PlayerTeam> Teams { get; set; }        
        public int Overall { get; set; }
        public int Potential { get; set; }
        public int Pace { get; set; }
        public int Shooting { get; set; }
        public int Passing { get; set;}
        public int Dribbling { get; set; }
        public int Defense { get; set; }
        public int Physical { get; set; }
    }
}
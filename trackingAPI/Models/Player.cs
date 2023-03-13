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
        //number of goals
        public int Goals { get; set; }
        //number of goals assisted
        public int Assists { get; set; }
        //Yellow Cards
        public int Yellow { get; set; }
        //Red Cards
        public int Red { get; set; }
        //Set piece goal
        public int SpG { get; set; }
        //penalties Scored
        public decimal PSPercent { get; set; }
        //Man of the match
        public int Motm { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace trackingAPI.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int age { get; set; }
        [ForeignKey("Team")]
        public Guid TeamId { get; set; }
        [NotMapped]
        public Team Team { get; set; }
    }
}
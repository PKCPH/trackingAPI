using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace trackingAPI.Models
{
    public class PlayerTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Player")]
        public Guid PlayerId { get; set; }
        [ForeignKey("Team")]
        public Guid TeamId { get; set; }
    }
}

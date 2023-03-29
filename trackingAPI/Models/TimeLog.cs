using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trackingAPI.Models
{
    public class TimeLog
    {

        public TimeLog()
        {
            this.Gamematch = new Gamematch();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }    

        public DateTime StartDateTime { get; set; }

        public TimeSpan TimeStamp { get; set; }

        public Gamematch? Gamematch { get; set; }

        public string Event { get; set; }
    }
}

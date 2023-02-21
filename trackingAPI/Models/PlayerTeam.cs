namespace trackingAPI.Models
{
    public class PlayerTeam
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
    }
}

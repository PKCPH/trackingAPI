using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trackingAPI.Models
{
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Balance { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Role { get; set; }
        public Login()
        {
            Email = "";
            Balance = "0";
            Id = Guid.NewGuid();
        }
    }
}

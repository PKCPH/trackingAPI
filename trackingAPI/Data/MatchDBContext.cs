using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using trackingAPI.Models;

namespace trackingAPI.Data;

public class MatchDBContext : DbContext
{
    public MatchDBContext(DbContextOptions<MatchDBContext> options) : base(options)
    {

    }

    public DbSet<Match> Matches { get; set; }
}

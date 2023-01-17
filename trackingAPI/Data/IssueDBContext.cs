using Microsoft.EntityFrameworkCore;
using trackingAPI.Models;

namespace trackingAPI.Data;

public class IssueDBContext : DbContext
{
    public IssueDBContext(DbContextOptions<IssueDBContext> options) : base(options)
    {
    }
    public DbSet<Issue> Issues { get; set; }
}

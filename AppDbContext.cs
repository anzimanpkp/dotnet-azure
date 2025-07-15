using Microsoft.EntityFrameworkCore;

namespace sample_web_app;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using TestTask.Core.Entities;

namespace TestTask.Infrastructure.SQL.Contexts;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<HumanName> HumanNames { get; set; }
    public DbSet<GivenName> GivenNames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

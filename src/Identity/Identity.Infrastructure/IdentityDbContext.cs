using Identity.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.Infrastructure;

public class IdentityDbContext : DbContext
{
    public DbSet<OrganizationDataModel> Organizations { get; set; }
    public DbSet<SubjectDataModel> Subjects { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
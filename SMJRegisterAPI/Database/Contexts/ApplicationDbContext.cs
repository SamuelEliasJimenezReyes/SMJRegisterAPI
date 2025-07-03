using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Entities;

namespace SMJRegisterAPI.Database.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Camper> Campers { get; set; }
    public DbSet<Church> Churches { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);    
    }
}
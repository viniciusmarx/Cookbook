using Cookbook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess;

public class CookbookDbContext(DbContextOptions<CookbookDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Testar se é necessario
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookbookDbContext).Assembly);

    }
}

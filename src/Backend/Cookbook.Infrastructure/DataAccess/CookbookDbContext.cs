using Cookbook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess;

public class CookbookDbContext(DbContextOptions<CookbookDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Recipe> Recipes { get; set; }
}

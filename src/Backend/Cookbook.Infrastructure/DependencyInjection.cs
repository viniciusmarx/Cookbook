using Cookbook.Domain.Interfaces.Repositories;
using Cookbook.Domain.Interfaces.Repositories.User;
using Cookbook.Infrastructure.DataAccess;
using Cookbook.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration.GetConnectionString("Connection")!);
        AddRepositories(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services, string connectionString)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        services.AddDbContext<CookbookDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }
}

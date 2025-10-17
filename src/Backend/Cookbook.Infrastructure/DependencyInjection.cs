using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.Cryptography;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Infrastructure.DataAccess;
using Cookbook.Infrastructure.DataAccess.Repositories;
using Cookbook.Infrastructure.Extensions;
using Cookbook.Infrastructure.Security.Cryptography;
using Cookbook.Infrastructure.Security.Tokens;
using Cookbook.Infrastructure.Services.LoggedUser;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cookbook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddJwtAuthentication(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncrypter(services);

        if (configuration.IsUnitTestEnvironment())
            return services;

        AddDbContext(services, configuration.ConnectionString());
        AddMigrations(services, configuration.ConnectionString());

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
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddMigrations(IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options.AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Cookbook.Infrastructure")).For.All();
        });
    }

    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddPasswordEncrypter(IServiceCollection services) => services.AddScoped<IPasswordEncripter, Sha512Encripter>();
}
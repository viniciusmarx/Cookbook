using AutoMapper;
using Cookbook.Application.Services.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Sqids;
using System.Reflection;

namespace Cookbook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddIdEncoder(services);
        AddAutoMapper(services);

        return services;
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("UseCase")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(options =>
        {
            var sqids = option.GetRequiredService<SqidsEncoder<long>>();
            options.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }

    private static void AddIdEncoder(IServiceCollection services)
    {
        var sqids = new SqidsEncoder<long>(new() { MinLength = 3 });
        services.AddSingleton(sqids);
    }
}
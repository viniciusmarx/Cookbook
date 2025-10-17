using AutoMapper;
using Cookbook.Application.Services.AutoMapper;
using Cookbook.Application.UseCases.Login;
using Cookbook.Application.UseCases.Recipe.Filter;
using Cookbook.Application.UseCases.Recipe.Register;
using Cookbook.Application.UseCases.User.ChangePassword;
using Cookbook.Application.UseCases.User.Profile;
using Cookbook.Application.UseCases.User.Register;
using Cookbook.Application.UseCases.User.Update;
using Microsoft.Extensions.DependencyInjection;
using Sqids;
using System.Reflection;

namespace Cookbook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
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
        var sqids = new SqidsEncoder<long>(new() { MinLength = 3 });
        services.AddScoped(option => new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }
}
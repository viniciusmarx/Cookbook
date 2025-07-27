using Cookbook.Application.Services.AutoMapper;
using Cookbook.Application.Services.Encryption;
using Cookbook.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Cookbook.Application.UseCases.Login;
using Cookbook.Application.UseCases.User.Profile;

namespace Cookbook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddAutoMapper(services);
        AddPasswordEncrypter(services);

        return services;
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped<PasswordEncripter>();
    }
}


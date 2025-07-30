using Cookbook.Application.Services.AutoMapper;
using Cookbook.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Cookbook.Application.UseCases.Login;
using Cookbook.Application.UseCases.User.Profile;
using Cookbook.Application.UseCases.User.Update;
using Cookbook.Application.UseCases.User.ChangePassword;

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
        services.AddScoped<IRegisterUser, RegisterUser>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }
}
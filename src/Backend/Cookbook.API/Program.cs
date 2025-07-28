using Cookbook.API.Converters;
using Cookbook.API.Filters;
using Cookbook.API.Middleware;
using Cookbook.API.Token;
using Cookbook.Application;
using Cookbook.Communication.Settings;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Infrastructure;
using Cookbook.Infrastructure.Extensions;
using Cookbook.Infrastructure.Migrations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then yor token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.Configure<PasswordSettings>(builder.Configuration.GetSection("Settings:Password"));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!builder.Configuration.IsUnitTestEnvironment())
{
    DatabaseMigration.Migrate(builder.Configuration.ConnectionString(), app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
}

app.Run();

public partial class Program
{
    protected Program() { }
}
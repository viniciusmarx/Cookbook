using Cookbook.API.Filters;
using Cookbook.API.Middleware;
using Cookbook.Application;
using Cookbook.Communication.Settings;
using Cookbook.Infrastructure;
using Cookbook.Infrastructure.Extensions;
using Cookbook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
.Services.AddEndpointsApiExplorer()
.AddSwaggerGen()
.Configure<PasswordSettings>(builder.Configuration.GetSection("Settings:Password"))
.AddApplication()
.AddInfrastructure(builder.Configuration)
.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

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
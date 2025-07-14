﻿using Microsoft.Extensions.Configuration;

namespace Cookbook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}

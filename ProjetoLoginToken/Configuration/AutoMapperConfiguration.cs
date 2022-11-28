using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace ProjetoLoginToken.Configuration;

public static class AutoMapperConfiguration
{
    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var assembly = Assembly.Load("ProjetoLoginToken");

        services
            .AddAutoMapper((serviceProvider, mapperConfiguration) => mapperConfiguration.AddMaps(assembly), assembly);
    }
}
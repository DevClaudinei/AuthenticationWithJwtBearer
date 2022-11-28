using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjetoLoginToken.Configuration;

public static class MvcConfiguration
{
    public static void AddMvcConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(options => options
            .RegisterValidatorsFromAssembly(Assembly.Load("ProjetoLoginToken")));
    }
}
using FSTest.Search.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FSTest.Search.Infrastructure
{
    public static class  ServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IChuckNorrisService, ChuckNorrisService>();
            services.AddScoped<IStarWarsService, StarWarsService>();
            return services;
        }
    }
}

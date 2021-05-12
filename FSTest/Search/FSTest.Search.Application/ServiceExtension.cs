using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using FSTest.Search.Application.Queries;

namespace FSTest.Search.Application
{
    public static class  ServiceExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
           // services.AddTransient<IValidator<SearchJokesAndPeopleQuery>, SearchJokesAndPeopleQueryValidator>();
            return services;
        }
    }
}

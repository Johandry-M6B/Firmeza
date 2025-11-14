using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class DependencyInjection
{
    public static IServiceCollection AddApplication(IServiceCollection services)
    {
        // Registrar MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Registrar AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registrar validadores de FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
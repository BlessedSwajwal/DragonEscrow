using Application.Common.Authorization;
using Application.Common.Behaviors;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjectionRegisterApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(typeof(DependencyInjectionRegisterApplication).Assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddValidatorsFromAssembly(typeof(DependencyInjectionRegisterApplication).Assembly);

        //Mapster
        services.AddSingleton<IMapper, ServiceMapper>();

        services.AddScoped<IAuthorizationHandler, OrderCreatorOrProviderHandler>();

        return services;
    }
}

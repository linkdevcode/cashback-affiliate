using System.Reflection;
using Cashback.Application.Behaviors;
using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cashback.Application;

/// <summary>
/// Application layer dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application layer services.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IWebhookIdempotencyService, WebhookIdempotencyService>();
        services.AddScoped<IWebhookProcessingService, WebhookProcessingService>();
        services.AddScoped<IWebhookUserResolver, WebhookSub1UserResolver>();
        services.AddScoped<IOrderSynchronizationService, OrderSynchronizationService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        services.AddAutoMapper(assembly);

        return services;
    }
}

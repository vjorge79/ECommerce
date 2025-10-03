using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Payments.Commands;

namespace PaymentService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR: registra handlers a partir de um tipo “âncora”
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>());

        // FluentValidation: registra validators do assembly
        services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();

        // Se tiver behaviors/pipelines, registre aqui (exemplos):
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
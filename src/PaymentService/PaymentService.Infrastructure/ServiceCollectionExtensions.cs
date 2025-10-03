using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Abstractions; // portas de saída (interfaces)
using PaymentService.Domain.Payments;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Persistence.Repositories;

namespace PaymentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext (SQL Server)
        services.AddDbContext<PaymentDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositórios / UoW
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PaymentDbContext>());

        // HealthChecks (opcional)
        // services.AddHealthChecks()
        //     .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);

        // Mensageria/Outbox/etc. (se houver)
        // services.AddMassTransit(...);
        // services.AddSingleton<IEventBus, MassTransitEventBus>();

        return services;
    }
}
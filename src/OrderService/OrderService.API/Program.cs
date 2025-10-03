using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderService.Application.Abstractions;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService API", Version = "v1" });
});

builder.Services.AddDbContext<OrderDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<OrderDbContext>());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentAuthorizedConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

    x.UsingRabbitMq((context, busCfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        var vhost = builder.Configuration["RabbitMq:VirtualHost"] ?? "/";
        var user = builder.Configuration["RabbitMq:Username"] ?? "guest";
        var pass = builder.Configuration["RabbitMq:Password"] ?? "guest";

        busCfg.Host(host, vhost, h =>
        {
            h.Username(user);
            h.Password(pass);
        });

        busCfg.UseMessageRetry(r => r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(3)));
        busCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Auto-migrate (dev)
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        db.Database.Migrate();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Verificar como adicionar certificado !!!
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
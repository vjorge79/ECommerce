using MassTransit;
using PaymentService.Application;
using PaymentService.Infrastructure;
using PaymentService.Worker.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication(); // This line will work if the using directive is correct

// DI da Infrastructure (DbContext, Repositories, Outbox, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// MassTransit + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "rabbitmq";
        var user = builder.Configuration["RabbitMq:Username"] ?? "guest";
        var pass = builder.Configuration["RabbitMq:Password"] ?? "guest";

        cfg.Host(host, h =>
        {
            h.Username(user);
            h.Password(pass);
        });

        cfg.ReceiveEndpoint("payment-order-created", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

var app = builder.Build();
await app.RunAsync();
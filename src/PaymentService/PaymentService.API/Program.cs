using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PaymentService.Application.Abstractions;
using PaymentService.Application.Payments.Commands;
using PaymentService.Domain.Payments;
using PaymentService.Infrastructure.Messaging;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Persistence.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentService API", Version = "v1" });
});

builder.Services.AddDbContext<PaymentDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>());

builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PaymentDbContext>());


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

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

// TODO: Adicionar Health Checks para o banco de dados !!!
//builder.Services.AddHealthChecks()
//    .AddSqlServer(builder.Configuration.GetConnectionString("PaymentDb") ??
//                  "Server=localhost,1433;Database=PaymentDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Auto-migrate (dev)
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
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

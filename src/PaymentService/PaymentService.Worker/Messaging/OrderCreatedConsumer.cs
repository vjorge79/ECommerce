using MassTransit;
using MediatR;
using Ordering.Contracts;
using PaymentService.Application.Payments.Commands;

namespace PaymentService.Worker.Messaging;

public sealed class OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IMediator mediator)
    : IConsumer<OrderCreatedV1>
{
    public async Task Consume(ConsumeContext<OrderCreatedV1> ctx)
    {
        var m = ctx.Message;

        logger.LogInformation("OrderCreatedV1 recebido: OrderId={OrderId} BuyerId={BuyerId} Total={Total}",
            m.OrderId, m.BuyerId, m.Total);

        // idempotência simples: como há índice único por OrderId, o 2º insert falha
        await mediator.Send(new CreatePaymentCommand(m.OrderId, m.BuyerId, m.Total), ctx.CancellationToken);
    }
}
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using OrderService.Application.Orders.Commands;

namespace OrderService.Infrastructure.Messaging;

public sealed class PaymentFailedConsumer(ILogger<PaymentFailedConsumer> logger, MediatR.IMediator mediator)
    : IConsumer<PaymentFailedV1>
{
    public async Task Consume(ConsumeContext<PaymentFailedV1> ctx)
    {
        logger.LogWarning("PaymentFailedV1 recebido: OrderId={OrderId} Reason={Reason}",
            ctx.Message.OrderId, ctx.Message.Reason);

        await mediator.Send(new MarkOrderPaymentFailedCommand(ctx.Message.OrderId), ctx.CancellationToken);
    }
}
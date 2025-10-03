using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Contracts;
using OrderService.Application.Orders.Commands;

namespace OrderService.Infrastructure.Messaging;

public sealed class PaymentAuthorizedConsumer(ILogger<PaymentAuthorizedConsumer> logger, MediatR.IMediator mediator)
    : IConsumer<PaymentAuthorizedV1>
{
    public async Task Consume(ConsumeContext<PaymentAuthorizedV1> ctx)
    {
        logger.LogInformation("PaymentAuthorizedV1 recebido: OrderId={OrderId}", ctx.Message.OrderId);

        await mediator.Send(new MarkOrderAsPaidCommand(ctx.Message.OrderId), ctx.CancellationToken);
    }
}
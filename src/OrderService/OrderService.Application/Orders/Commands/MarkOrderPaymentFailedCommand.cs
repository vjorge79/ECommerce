using MediatR;

namespace OrderService.Application.Orders.Commands;

public sealed record MarkOrderPaymentFailedCommand(Guid OrderId) : IRequest<bool>;
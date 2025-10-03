using MediatR;

namespace OrderService.Application.Orders.Commands;

public sealed record MarkOrderAsPaidCommand(Guid OrderId) : IRequest<bool>;
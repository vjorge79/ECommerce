using MediatR;

namespace OrderService.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid Id) : IRequest<bool>;
using MediatR;
using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Orders.Queries;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;
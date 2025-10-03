using MediatR;
using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Orders.Commands;

public sealed record CreateOrderCommand(
    string BuyerId,
    ShipToAddressDto ShipToAddress,
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;
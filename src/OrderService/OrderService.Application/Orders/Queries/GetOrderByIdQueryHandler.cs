using MediatR;
using OrderService.Application.Orders.DTOs;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Queries;

public sealed class GetOrderByIdQueryHandler(IOrderRepository repo)
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await repo.GetByIdAsync(request.Id, ct);

        if (order is null) return null;

        var address = order.ShipToAddress;
        var items = order.OrderItems ?? [];

        var itemDtos = items.Select(i => new OrderItemDto(
            i.Id,
            i.ItemOrdered.CatalogItemId,
            i.ItemOrdered.ProductName,
            i.ItemOrdered.PictureUri,
            i.UnitPrice,
            i.Units,
            i.UnitPrice * i.Units
        )).ToList();

        var status = GetStatus(order.Status);

        return new OrderDto(
            order.Id,
            order.BuyerId,
            order.OrderDate,
            address?.Street ?? string.Empty,
            address?.City ?? string.Empty,
            address?.State ?? string.Empty,
            address?.Country ?? string.Empty,
            address?.ZipCode ?? string.Empty,
            order.Total(),
            itemDtos,
            status
        );
    }

    private static string GetStatus(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Created => "Created",
            OrderStatus.PendingPayment => "Pending Payment",
            OrderStatus.Paid => "Paid",
            OrderStatus.PaymentFailed => "Payment Failed",
            OrderStatus.Shipped => "Shipped",
            OrderStatus.Cancelled => "Cancelled",
            _ => "Unknown"
        };
    }
}
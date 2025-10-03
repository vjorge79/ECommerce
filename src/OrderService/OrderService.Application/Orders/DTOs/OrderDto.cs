namespace OrderService.Application.Orders.DTOs;

public sealed record OrderDto(
    Guid Id,
    string BuyerId,
    DateTimeOffset OrderDate,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    decimal Total,
    IEnumerable<OrderItemDto> Items,
    string Status
);
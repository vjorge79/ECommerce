namespace OrderService.Application.Orders.DTOs;

public sealed record OrderItemDto(
    Guid Id,
    int CatalogItemId,
    string ProductName,
    string PictureUri,
    decimal UnitPrice,
    int Units,
    decimal Subtotal
);
namespace OrderService.Application.Orders.DTOs;

public sealed record CreateOrderItemDto(
    int CatalogItemId,
    string ProductName,
    string PictureUri,
    decimal UnitPrice,
    int Units
);
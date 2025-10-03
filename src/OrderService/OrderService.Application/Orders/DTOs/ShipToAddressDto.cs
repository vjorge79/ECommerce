namespace OrderService.Application.Orders.DTOs;

public sealed record ShipToAddressDto(string Street, string City, string State, string Country, string ZipCode);
using MassTransit;
using MediatR;
using Ordering.Contracts;
using OrderService.Application.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Commands;

public sealed class CreateOrderCommandHandler(
    IOrderRepository repo,
    IUnitOfWork uow,
    IPublishEndpoint bus
) : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var address = new Address(
                request.ShipToAddress.Street,
                request.ShipToAddress.City,
                request.ShipToAddress.State,
                request.ShipToAddress.Country,
                request.ShipToAddress.ZipCode);

        var items = request.Items.Select(i => new OrderItem(
            new CatalogItemOrdered(i.CatalogItemId, i.ProductName, i.PictureUri),
            i.UnitPrice,
            i.Units
        ));

        var order = new Order(request.BuyerId, address, [.. items]);

        await repo.AddAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        var evt = new OrderCreatedV1(
            MessageId: Guid.NewGuid().ToString(),
            OccurredAtUtc: DateTime.UtcNow,
            OrderId: order.Id,
            BuyerId: order.BuyerId,
            Total: order.Total()
        );

        await bus.Publish(evt, ct);
        return order.Id;
    }
}
using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Commands;

public sealed class DeleteOrderCommandHandler(IOrderRepository repo, IUnitOfWork uow)
    : IRequestHandler<DeleteOrderCommand, bool>
{
    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken ct)
    {
        var order = await repo.GetByIdAsync(request.Id, ct);

        if (order is null) return false;

        repo.Remove(order);
        await uow.SaveChangesAsync(ct);

        return true;
    }
}
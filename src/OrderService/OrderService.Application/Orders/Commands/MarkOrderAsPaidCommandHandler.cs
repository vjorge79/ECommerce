using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Commands;

public sealed class MarkOrderAsPaidCommandHandler(IOrderRepository repo, IUnitOfWork uow)
    : IRequestHandler<MarkOrderAsPaidCommand, bool>
{
    public async Task<bool> Handle(MarkOrderAsPaidCommand request, CancellationToken ct)
    {
        var order = await repo.GetAggregateByIdAsync(request.OrderId, ct);

        if (order is null) return false;

        order.MarkAsPaid();
        await uow.SaveChangesAsync(ct);

        return true;
    }
}
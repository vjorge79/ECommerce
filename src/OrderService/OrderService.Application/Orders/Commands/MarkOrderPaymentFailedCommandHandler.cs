using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Commands;

public sealed class MarkOrderPaymentFailedCommandHandler(IOrderRepository repo, IUnitOfWork uow)
    : IRequestHandler<MarkOrderPaymentFailedCommand, bool>
{
    public async Task<bool> Handle(MarkOrderPaymentFailedCommand request, CancellationToken ct)
    {
        var order = await repo.GetAggregateByIdAsync(request.OrderId, ct);

        if (order is null) return false;

        order.MarkPaymentFailed();
        await uow.SaveChangesAsync(ct);

        return true;
    }
}
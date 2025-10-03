using MassTransit;
using MediatR;
using Ordering.Contracts;
using PaymentService.Application.Abstractions;
using PaymentService.Domain.Payments;

namespace PaymentService.Application.Payments.Commands;

public sealed class CreatePaymentCommandHandler(
    IPaymentRepository repo,
    IUnitOfWork uow,
    IPublishEndpoint bus
) : IRequestHandler<CreatePaymentCommand, Guid>
{
    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken ct)
    {
        var payment = new Payment(request.OrderId, request.BuyerId, request.Amount);

        // TODO: Implementar processo de pagamento !!!
        payment.Authorize(); // mock: autoriza de imediato (troque pelo gateway real depois)

        await repo.AddAsync(payment, ct);
        await uow.SaveChangesAsync(ct);

        if (payment.Status == PaymentStatus.Authorized)
        {
            await bus.Publish(new PaymentAuthorizedV1(
                MessageId: Guid.NewGuid().ToString(),
                OccurredAtUtc: DateTime.UtcNow,
                OrderId: payment.OrderId,
                BuyerId: payment.BuyerId,
                Amount: payment.Amount
            ), ct);
        }
        else
        {
            await bus.Publish(new PaymentFailedV1(
                MessageId: Guid.NewGuid().ToString(),
                OccurredAtUtc: DateTime.UtcNow,
                OrderId: payment.OrderId,
                BuyerId: payment.BuyerId,
                Reason: "Mock gateway refused"
            ), ct);
        }

        return payment.Id;
    }
}
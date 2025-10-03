using MediatR;
using PaymentService.Domain.Payments;

namespace PaymentService.Application.Payments.Queries;

public sealed class GetPaymentByOrderIdQueryHandler(IPaymentRepository repo)
    : IRequestHandler<GetPaymentByOrderIdQuery, Payment?>
{
    public Task<Payment?> Handle(GetPaymentByOrderIdQuery request, CancellationToken ct)
        => repo.GetByOrderIdAsync(request.OrderId, ct);
}
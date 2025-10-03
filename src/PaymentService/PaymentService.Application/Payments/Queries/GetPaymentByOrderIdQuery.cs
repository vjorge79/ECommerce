using MediatR;
using PaymentService.Domain.Payments;

namespace PaymentService.Application.Payments.Queries;

public sealed record GetPaymentByOrderIdQuery(Guid OrderId) : IRequest<Payment?>;
using MediatR;

namespace PaymentService.Application.Payments.Commands;

public sealed record CreatePaymentCommand(Guid OrderId, string BuyerId, decimal Amount) : IRequest<Guid>;
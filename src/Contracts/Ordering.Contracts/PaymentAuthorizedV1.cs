namespace Ordering.Contracts;

public sealed record PaymentAuthorizedV1(
    string MessageId,
    DateTime OccurredAtUtc,
    Guid OrderId,
    string BuyerId,
    decimal Amount
);
namespace Ordering.Contracts;

public sealed record PaymentFailedV1(
    string MessageId,
    DateTime OccurredAtUtc,
    Guid OrderId,
    string BuyerId,
    string Reason
);
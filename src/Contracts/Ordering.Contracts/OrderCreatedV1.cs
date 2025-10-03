namespace Ordering.Contracts;

public sealed record OrderCreatedV1(
    string MessageId,
    DateTime OccurredAtUtc,
    Guid OrderId,
    string BuyerId,
    decimal Total
);
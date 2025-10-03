namespace PaymentService.Domain.Payments;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public string BuyerId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public DateTimeOffset CreatedAtUtc { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? AuthorizedAtUtc { get; private set; }

    private Payment() { }

    public Payment(Guid orderId, string buyerId, decimal amount)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId inválido");
        if (string.IsNullOrWhiteSpace(buyerId)) throw new ArgumentException("BuyerId inválido");
        if (amount <= 0) throw new ArgumentException("Amount deve ser > 0");

        OrderId = orderId;
        BuyerId = buyerId.Trim();
        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
    }

    public void Authorize()
    {
        if (Status != PaymentStatus.Pending) return;
        Status = PaymentStatus.Authorized;
        AuthorizedAtUtc = DateTimeOffset.UtcNow;
    }

    public void Fail() => Status = PaymentStatus.Failed;
}
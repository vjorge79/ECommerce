namespace OrderService.Domain.Orders;

public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string BuyerId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.UtcNow;
    public Address ShipToAddress { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

#pragma warning disable CS8618
    private Order() { }

    public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
    {
        // TODO: Verificar como validar da melhor forma !!!
        //Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));

        BuyerId = buyerId;
        ShipToAddress = shipToAddress;
        _orderItems = items;
    }

    public decimal Total()
    {
        var total = 0m;
        foreach (var item in _orderItems)
        {
            total += item.UnitPrice * item.Units;
        }
        return total;
    }

    // após criar (ou no handler de criação), você pode mover para PendingPayment:
    public void MarkPendingPayment()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Estado inválido para pendente de pagamento.");
        Status = OrderStatus.PendingPayment;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created && Status != OrderStatus.PendingPayment)
            throw new InvalidOperationException("Somente pedidos recém-criados ou pendentes podem ser pagos.");
        Status = OrderStatus.Paid;
    }

    public void MarkPaymentFailed()
    {
        if (Status != OrderStatus.Created && Status != OrderStatus.PendingPayment)
            throw new InvalidOperationException("Somente pedidos aguardando pagamento podem falhar.");
        Status = OrderStatus.PaymentFailed;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Somente pedidos pagos podem ser enviados.");
        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Cancelled)
            throw new InvalidOperationException("Não é possível cancelar pedido enviado ou já cancelado.");
        Status = OrderStatus.Cancelled;
    }
}
namespace OrderService.Domain.Orders;

public class OrderItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public CatalogItemOrdered ItemOrdered { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Units { get; private set; }

#pragma warning disable CS8618
    private OrderItem() { }

    public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
    {
        ItemOrdered = itemOrdered;
        UnitPrice = unitPrice;
        Units = units;
    }
}
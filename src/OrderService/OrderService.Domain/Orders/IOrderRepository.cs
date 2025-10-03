namespace OrderService.Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Order?> GetAggregateByIdAsync(Guid id, CancellationToken ct = default);
    void Remove(Order order);
}
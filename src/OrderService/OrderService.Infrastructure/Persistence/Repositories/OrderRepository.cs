using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository(OrderDbContext db) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken ct = default)
        => await db.Orders.AddAsync(order, ct);

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Orders.Include(o => o.OrderItems).AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<Order?> GetAggregateByIdAsync(Guid id, CancellationToken ct = default)
        => await db.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id, ct);

    public void Remove(Order order)
        => db.Orders.Remove(order);
}
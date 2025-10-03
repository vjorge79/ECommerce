using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Payments;

namespace PaymentService.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository(PaymentDbContext db) : IPaymentRepository
{
    public async Task AddAsync(Payment entity, CancellationToken ct = default) => await db.Payments.AddAsync(entity, ct);
    public Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
        => db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.OrderId == orderId, ct);
    public Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
}

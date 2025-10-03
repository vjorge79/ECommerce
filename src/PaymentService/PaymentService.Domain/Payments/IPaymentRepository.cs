namespace PaymentService.Domain.Payments;

public interface IPaymentRepository
{
    Task AddAsync(Payment entity, CancellationToken ct = default);
    Task<Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default);
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
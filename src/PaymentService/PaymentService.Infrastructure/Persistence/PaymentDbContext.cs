using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Abstractions;
using PaymentService.Domain.Payments;

namespace PaymentService.Infrastructure.Persistence;

public sealed class PaymentDbContext(DbContextOptions<PaymentDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(b =>
        {
            b.ToTable("Payments");
            b.HasKey(x => x.Id);
            b.Property(x => x.OrderId).IsRequired();
            b.Property(x => x.BuyerId).HasMaxLength(100).IsRequired();
            b.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.HasIndex(x => x.OrderId).IsUnique(); // 1 pagamento por pedido (ajuste se necessário)
        });
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => base.SaveChangesAsync(ct);
}
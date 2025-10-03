using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Persistence;

public sealed class OrderDbContext(DbContextOptions<OrderDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            b.ToTable("Orders");
            b.HasKey(x => x.Id);

            b.Property(x => x.BuyerId).HasMaxLength(100).IsRequired();
            b.Property(x => x.OrderDate).IsRequired();

            // Address (VO) como owned
            b.OwnsOne(x => x.ShipToAddress, ab =>
            {
                ab.Property(a => a.Street).HasMaxLength(200).IsRequired();
                ab.Property(a => a.City).HasMaxLength(100).IsRequired();
                ab.Property(a => a.State).HasMaxLength(100).IsRequired();
                ab.Property(a => a.Country).HasMaxLength(100).IsRequired();
                ab.Property(a => a.ZipCode).HasMaxLength(20).IsRequired();
                ab.ToTable("Orders"); // mesmas colunas na tabela principal
            });

            b.Property(x => x.Status).IsRequired();

            // Itens (owned collection)
            b.OwnsMany(x => x.OrderItems, ib =>
            {
                ib.ToTable("OrderItems");
                ib.WithOwner().HasForeignKey("OrderId");
                ib.HasKey("Id");
                ib.Property<Guid>("Id");
                ib.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();
                ib.Property(i => i.Units).IsRequired();

                // VO interno: CatalogItemOrdered
                ib.OwnsOne(i => i.ItemOrdered, ob =>
                {
                    ob.Property(o => o.CatalogItemId).IsRequired();
                    ob.Property(o => o.ProductName).HasMaxLength(200).IsRequired();
                    ob.Property(o => o.PictureUri).HasMaxLength(500).IsRequired();
                });

                ib.HasIndex("OrderId");
            });
        });
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}
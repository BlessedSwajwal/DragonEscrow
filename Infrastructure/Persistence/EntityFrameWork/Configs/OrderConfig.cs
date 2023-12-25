using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFrameWork.Configs;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, val => OrderId.Create(val));

        builder.Property(o => o.ConsumerId)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.Property(o => o.ProviderId)
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.OwnsMany(o => o.Bids, bb =>
        {
            bb.ToTable("Order-Bids");
            bb.WithOwner().HasForeignKey("OrderId");
            bb.HasKey("Id", "OrderId");

            bb.Property(o => o.Id)
                .ValueGeneratedNever()
                .HasColumnName("BidId")
                .HasConversion(id => id.Value, val => BidId.Create(val));
        });

        builder.Navigation(o => o.Bids)
            .HasField("_bids")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(o => o.AcceptedBid, ab =>
        {
            ab.ToTable("Order-AcceptedBid");
            ab.WithOwner().HasForeignKey("BidId");
            ab.HasKey("Id", "BidId");

            ab.Property(a => a.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, val => BidId.Create(val));

        });
    }
}

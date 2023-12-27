using Domain.Bids;
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
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.OwnsMany(o => o.BidIds, bb =>
        {
            bb.ToTable("Order-Bids");
            bb.WithOwner().HasForeignKey("OrderId");
            bb.HasKey("Value", "OrderId");

            bb.Property(x => x.Value)
                .ValueGeneratedNever();
        });

        builder.Navigation(o => o.BidIds)
            .HasField("_bidIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        //builder.HasOne(o => o.AcceptedBid);

        builder.Property(o => o.AcceptedBidId)
            .HasConversion(id => id.Value, val => BidId.Create(val));
    }
}

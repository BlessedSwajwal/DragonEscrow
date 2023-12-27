using Domain.Bids;
using Domain.Order.ValueObjects;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFrameWork.Configs;

public class BidConfig : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, val => BidId.Create(val));

        builder.Property(x => x.OrderId)
            .HasConversion(id => id.Value, val => OrderId.Create(val));

        builder.HasIndex(x => x.OrderId)
            .IsUnique(false);

        builder.Property(x => x.BidderId)
            .HasConversion(id => id.Value, val => UserId.Create(val));
    }
}

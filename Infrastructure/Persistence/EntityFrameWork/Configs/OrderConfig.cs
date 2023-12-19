using Domain.Order;
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

        //builder.Property(o => o.Status)
        //    .ValueGeneratedNever()
        //    .HasC

        builder.Property(o => o.ConsumerId)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.Property(o => o.ProviderId)
            .HasConversion(id => id.Value, val => UserId.Create(val));

    }
}

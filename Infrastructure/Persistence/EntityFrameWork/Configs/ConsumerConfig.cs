using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFrameWork.Configs;

public class ConsumerConfig : IEntityTypeConfiguration<Consumer>
{
    public void Configure(EntityTypeBuilder<Consumer> builder)
    {
        builder.ToTable("Consumers");
        builder.HasKey(e => e.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .HasColumnName("ConsumerId")
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.OwnsMany(c => c.OrderIds, ob =>
        {
            ob.ToTable("Consumer_OrderId");
            ob.WithOwner().HasForeignKey("ConsumerId");
            ob.HasKey("Value", "ConsumerId");
            ob.Property(x => x.Value).ValueGeneratedNever().HasColumnName("Value");
        });
    }
}

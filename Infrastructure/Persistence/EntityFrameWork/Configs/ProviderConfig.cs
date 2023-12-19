using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFrameWork.Configs;

public class ProviderConfig : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");
        builder.HasKey(e => e.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .HasColumnName("ProviderId")
            .HasConversion(id => id.Value, val => UserId.Create(val));

        builder.OwnsMany(c => c.AcceptedOrders, ob =>
        {
            ob.ToTable("Provider_AcceptedOrderId");
            ob.WithOwner().HasForeignKey("ProviderId");
            ob.HasKey("Value", "ProviderId");
            ob.Property(x => x.Value).ValueGeneratedNever().HasColumnName("Value");
        });
    }
}

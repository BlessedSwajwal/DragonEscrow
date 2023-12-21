using Domain.Common;
using Domain.Order;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork;

public class DragonEscrowDbContext : DbContext
{
    public DragonEscrowDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<Consumer> Consumers { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(DragonEscrowDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

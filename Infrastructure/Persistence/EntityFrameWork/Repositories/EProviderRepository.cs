using Application.Common.Repositories;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EProviderRepository : IProviderRepository
{
    private readonly DragonEscrowDbContext _dbContext;

    public EProviderRepository(DragonEscrowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Provider> AddProvider(Provider provider)
    {
        await _dbContext.Providers.AddAsync(provider);
        return provider;
    }

    public async Task<bool> CheckIfAlreadyBiddedAsync(UserId userId, OrderId orderId)
    {
        var user = await _dbContext.Providers.FindAsync(userId);
        if (user is null) return true;
        if (user.AcceptedOrders.Contains(orderId)) return true;
        return false;
    }

    public async Task<IReadOnlyList<Order>> GetConsumersOrder(UserId id)
    {
        var orders = await _dbContext.Orders.Where(o => o.ProviderId == id)
                                        .ToListAsync();
        return orders;
    }

    public async Task<Provider> GetByEmail(string email)
    {
        var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Email == email);
        if (provider is null) return Provider.Empty;
        return provider;
    }

    public async Task<Provider> GetByIdAsync(UserId id)
    {
        var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id);
        if (provider is null) return Provider.Empty;
        return provider;
    }

    public async Task<List<Provider>> GetAllByIdAsync(List<UserId> ids)
    {
        var providerList = await _dbContext.Providers.Where(p => ids.Contains(p.Id)).ToListAsync();
        return providerList;
    }
}

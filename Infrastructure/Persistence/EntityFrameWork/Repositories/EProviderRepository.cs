using Application.Common.Repositories;
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

    public async Task<Provider> GetByEmail(string email)
    {
        var provider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.Email == email);
        if (provider is null) return Provider.Empty;
        return provider;
    }
}

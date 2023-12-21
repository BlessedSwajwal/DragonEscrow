using Application.Common.Repositories;
using Domain.User;

namespace Infrastructure.Persistence;

public class ProviderRepository : IProviderRepository
{
    private static readonly List<Provider> _providers = new List<Provider>();
    public async Task<Provider> AddProvider(Provider provider)
    {
        await Task.CompletedTask;
        _providers.Add(provider);
        return provider;
    }

    public async Task<Provider> GetByEmail(string email)
    {
        await Task.CompletedTask;
        var provider = _providers.FirstOrDefault(p => p.Email == email);
        if (provider is null) return Provider.Empty;
        return provider;
    }

    public Task<Provider> GetByIdAsync(UserId id)
    {
        throw new NotImplementedException();
    }
}

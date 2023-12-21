using Domain.User;

namespace Application.Common.Repositories;

public interface IProviderRepository
{
    Task<Provider> AddProvider(Provider provider);
    Task<Provider> GetByEmail(string email);
    Task<Provider> GetByIdAsync(UserId id);
}

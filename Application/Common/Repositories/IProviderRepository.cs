using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;

namespace Application.Common.Repositories;

public interface IProviderRepository
{
    Task<Provider> AddProvider(Provider provider);
    Task<bool> CheckIfAlreadyBiddedAsync(UserId userId, OrderId orderId);
    Task<List<Provider>> GetAllByIdAsync(List<UserId> ids);
    Task<Provider> GetByEmail(string email);
    Task<Provider> GetByIdAsync(UserId id);

    Task<IReadOnlyList<Order>> GetConsumersOrder(UserId id);
}

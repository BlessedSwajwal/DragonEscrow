using Domain.User;

namespace Application.Common.Repositories;

public interface IConsumerRepository
{
    Task<Consumer> AddConsumer(Consumer consumer);
    Task<Consumer> GetByEmail(string email);

    Task<Consumer> GetByIdAsync(UserId id);
}

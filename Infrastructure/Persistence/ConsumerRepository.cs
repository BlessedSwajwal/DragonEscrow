using Application.Common.Repositories;
using Domain.User;

namespace Infrastructure.Persistence;

public class ConsumerRepository : IConsumerRepository
{
    private static readonly List<Consumer> _consumers = new List<Consumer>();
    public async Task<Consumer> AddConsumer(Consumer consumer)
    {
        _consumers.Add(consumer);
        return consumer;
    }

    public async Task<Consumer> GetByEmail(string email)
    {
        var consumer = _consumers.FirstOrDefault(c => c.Email == email);
        if (consumer is null) return Consumer.Empty;
        return consumer;
    }

    public Task<Consumer> GetByIdAsync(UserId id)
    {
        throw new NotImplementedException();
    }
}

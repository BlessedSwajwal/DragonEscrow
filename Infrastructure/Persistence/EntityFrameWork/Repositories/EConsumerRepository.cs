using Application.Common.Repositories;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EConsumerRepository : IConsumerRepository
{
    private readonly DragonEscrowDbContext _dbContext;
    public EConsumerRepository(DragonEscrowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Consumer> AddConsumer(Consumer consumer)
    {
        await _dbContext.Consumers.AddAsync(consumer);
        return consumer;
    }

    public async Task<Consumer> GetByEmail(string email)
    {
        var user = await _dbContext.Consumers.FirstOrDefaultAsync(c => c.Email == email);
        if (user is null) { return Consumer.Empty; }
        return user;
    }

    public async Task<Consumer> GetByIdAsync(UserId id)
    {
        var consumer = await _dbContext.Consumers.FirstOrDefaultAsync(u => u.Id == id);
        if (consumer is null) { return Consumer.Empty; };
        return consumer;
    }
}

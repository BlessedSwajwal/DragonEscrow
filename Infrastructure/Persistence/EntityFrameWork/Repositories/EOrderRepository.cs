using Application.Common.Repositories;
using Domain.Order;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EOrderRepository(DragonEscrowDbContext _dbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }
}

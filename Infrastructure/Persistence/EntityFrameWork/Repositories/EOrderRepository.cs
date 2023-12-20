using Application.Common.Repositories;
using Domain.Order;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EOrderRepository(DragonEscrowDbContext _dbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    public async Task<Order> GetOrderByIdAsync(OrderId id)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return Order.Empty;
        return order;
    }
}

using Application.Common.Repositories;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EOrderRepository(DragonEscrowDbContext _dbContext) : IOrderRepository
{
    public async Task<Order> AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    public async Task<List<Order>> GetAllOrdersAsyncFromConsumerId(UserId id)
    {
        var orders = await _dbContext.Orders.Where(o => o.ConsumerId == id)
                                            .OrderBy(o => o.OrderStatus)
                                            .ToListAsync();
        return orders;
    }

    public async Task<List<Order>> GetCreatedOrders()
    {
        var orders = _dbContext.Orders.Where(o => o.OrderStatus.ToLower().Equals(OrderStatusConstants.CREATED))
                                .OrderByDescending(o => o.Cost);
        return await orders.ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(OrderId id)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return Order.Empty;
        return order;
    }
}

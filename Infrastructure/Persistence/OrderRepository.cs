using Application.Common.Repositories;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;

namespace Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly static List<Order> _orders = new List<Order>();
    public async Task<Order> AddAsync(Order order)
    {
        await Task.CompletedTask;
        _orders.Add(order);
        return order;
    }

    public Task<List<Order>> GetAllOrdersAsyncFromConsumerId(UserId id)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrderByIdAsync(OrderId id)
    {
        await Task.CompletedTask;
        var order = _orders.FirstOrDefault(x => x.Id == id);
        if (order is null) return Order.Empty;
        return order;
    }
}

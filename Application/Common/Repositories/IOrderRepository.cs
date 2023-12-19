using Domain.Order;

namespace Application.Common.Repositories;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
}

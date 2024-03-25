using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;

namespace Application.Common.Repositories;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order> GetOrderByIdAsync(OrderId id);

    Task<List<Order>> GetAllOrdersAsyncFromConsumerId(UserId id);
    Task<List<Order>> GetCreatedOrders();
    Task<IReadOnlyList<Order>> GetCompletedOrders();
    Task<IReadOnlyList<Order>> GetDisputedOrders();
    Task<double> GetMonthlyTransactions();
    Task<double> GetPreviousMonthTransaction();
    Task<object> GetOverAllDetails();
    Task MarkOrderPaid(OrderId orderId);
    Task<IReadOnlyList<Order>> GetPaidOrders();
}

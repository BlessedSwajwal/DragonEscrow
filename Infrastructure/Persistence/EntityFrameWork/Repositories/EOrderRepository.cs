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

    public async Task<IReadOnlyList<Order>> GetCompletedOrders()
    {
        var completedOrders = await _dbContext.Orders.Where(or => or.OrderStatus.Equals(OrderStatusConstants.COMPLETED)).ToListAsync();

        return completedOrders.AsReadOnly();
    }

    public async Task<IReadOnlyList<Order>> GetDisputedOrders()
    {
        var completedOrders = await _dbContext.Orders.Where(or => or.OrderStatus.Equals(OrderStatusConstants.DISPUTED)).ToListAsync();

        return completedOrders.AsReadOnly();
    }

    public async Task<IReadOnlyList<Order>> GetPaidOrders()
    {
        var paidOrders = await _dbContext.Orders.Where(or => or.OrderStatus.Equals(OrderStatusConstants.PAID)).ToListAsync();

        return paidOrders.AsReadOnly();
    }

    public async Task<double> GetMonthlyTransactions()
    {
        var lastMonthSelectedBidIds = await _dbContext.Orders.Where(or => !or.OrderStatus.Equals(OrderStatusConstants.PENDING) &&
                or.AcceptedDate > DateTime.Now.AddDays(-30))
                    .Select((or) => or.AcceptedBidId).ToArrayAsync();

        var amount = _dbContext.Bids.Where(b => lastMonthSelectedBidIds.Contains(b.Id)).Sum(b => b.ProposedAmount);
        return amount / 100;
    }

    public async Task<double> GetPreviousMonthTransaction()
    {
        var prevMonthSelectedBidIds = await _dbContext.Orders.Where(or => !or.OrderStatus.Equals(OrderStatusConstants.PENDING) &&
                or.AcceptedDate < DateTime.Now.AddDays(-30) && or.AcceptedDate > DateTime.Now.AddDays(-30))
                    .Select((or) => or.AcceptedBidId).ToArrayAsync();

        var amount = _dbContext.Bids.Where(b => prevMonthSelectedBidIds.Contains(b.Id)).Sum(b => b.ProposedAmount);
        return amount / 100;
    }

    public async Task<Object> GetOverAllDetails()
    {
        await Task.CompletedTask;
        var totalDisputedOrders = _dbContext.Orders.Count(or => or.OrderStatus.Equals(OrderStatusConstants.DISPUTED));
        var totalPaidOrders = _dbContext.Orders.Count(or => or.OrderStatus.Equals(OrderStatusConstants.PAID));
        var totalCompletedOrders = _dbContext.Orders.Count(or => or.OrderStatus.Equals(OrderStatusConstants.COMPLETED));
        var totalConsumers = _dbContext.Consumers.Count();
        var totalProviders = _dbContext.Providers.Count();

        return new
        {
            totalConsumers,
            totalProviders,
            totalDisputedOrders,
            totalCompletedOrders,
            totalPaidOrders,
        };
    }

    public async Task MarkOrderPaid(OrderId orderId)
    {
        _dbContext.Orders.Single(or => or.Id == orderId).ChangeStatus(OrderStatusConstants.PAID);
        await _dbContext.SaveChangesAsync();
    }
}

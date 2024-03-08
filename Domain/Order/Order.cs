using Domain.Bids;
using Domain.Common;
using Domain.Order.Events;
using Domain.Order.ValueObjects;
using Domain.User;

namespace Domain.Order;

public class Order : Entity<OrderId>
{
    public static Order Empty = new Order(OrderId.Create(Guid.Empty), "", "", 1, null, 0);
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Cost { get; private set; }
    //TODO: change this to OrderStatus.PENDING. Khalti not working for now.
    public string OrderStatus { get; private set; } = OrderStatusConstants.CREATED;
    public UserId ConsumerId { get; private set; }
    public int AllowedDays { get; private set; }
    public UserId ProviderId { get; private set; } = UserId.Create(Guid.Empty);
    public DateTime AcceptedDate { get; private set; } = DateTime.MinValue;
    public DateTime DeadLine => AcceptedDate.AddDays(AllowedDays);
    public DateTime CompletionDate { get; private set; } = DateTime.MinValue;
    private List<BidId> _bidIds = [];
    public IReadOnlyList<BidId> BidIds => _bidIds.AsReadOnly();
    public BidId? AcceptedBidId { get; private set; }
    public bool Rated { get; private set; } = false;
    public int Rating { get; private set; } = 0;

    private Order(OrderId id, string name, string description, int cost, UserId consumerId, int allowedDays) : base(id)
    {
        Name = name;
        Description = description;
        Cost = cost;
        ConsumerId = consumerId;
        AllowedDays = allowedDays;
    }

    public static Order Create(string name, string description, int cost, UserId consumerId, int allowedDays)
    {
        var order = new Order(
                OrderId.CreateUnique(),
                name,
                description,
                cost,
                consumerId,
                allowedDays);

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    /// <summary>
    /// Change status of the order
    /// </summary>
    /// <param name="status"></param>
    public void ChangeStatus(string status)
    {
        OrderStatus = status;
    }

    /// <summary>
    /// Assign provider to the order. Deadline is created as soon as provider is associated.
    /// </summary>
    /// <param name="providerId"></param>
    public void AssignProvider(UserId providerId)
    {
        ProviderId = providerId;
        AcceptedDate = DateTime.UtcNow;

        this.AddDomainEvent(new OrderAcceptedEvent(this, providerId));
    }

    public void AddBid(BidId bid)
    {
        _bidIds.Add(bid);
    }

    public void MarkComplete()
    {
        CompletionDate = DateTime.UtcNow;
        //TODO: Order created event
    }

    //TODO: should be called in event handler. Event should be raised after bid was accepted.
    public void AcceptBid(Bid bid)
    {
        AcceptedBidId = bid.Id;
        ProviderId = bid.BidderId;
        AcceptedDate = DateTime.UtcNow;
        OrderStatus = OrderStatusConstants.PROCESSING;
    }

    //Rate
    public void RateOrder(int ratingCount)
    {
        Rated = true;
        Rating = ratingCount;
    }
    private Order() { }
}

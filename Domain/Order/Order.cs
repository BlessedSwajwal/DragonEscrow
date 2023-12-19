using Domain.Common;
using Domain.User;

namespace Domain.Order;

public class Order : Entity<OrderId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.PENDING;
    public UserId ConsumerId { get; private set; }
    public int AllowedDays { get; private set; }
    public UserId ProviderId { get; private set; } = UserId.Create(Guid.Empty);
    public DateTime AcceptedDate { get; private set; } = DateTime.MinValue;
    public DateTime DeadLine => AcceptedDate.AddDays(AllowedDays);

    private Order(OrderId id, string name, string description, UserId consumerId, int allowedDays) : base(id)
    {
        Name = name;
        Description = description;
        ConsumerId = consumerId;
        AllowedDays = allowedDays;
    }

    public static Order Create(string name, string description, UserId consumerId, int allowedDays)
    {
        return new(
                OrderId.CreateUnique(),
                name,
                description,
                consumerId, allowedDays);
    }

    /// <summary>
    /// Change status of the order
    /// </summary>
    /// <param name="status"></param>
    public void ChangeStatus(OrderStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Assign provider to the order. Deadline is created as soon as provider is associated.
    /// </summary>
    /// <param name="providerId"></param>
    public void AssignProvider(UserId providerId)
    {
        ProviderId = providerId;
        AcceptedDate = DateTime.UtcNow;
    }

    private Order() { }
}

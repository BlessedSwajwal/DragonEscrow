using Domain.Order;

namespace Domain.User;

public class Consumer : UserBase
{
    public readonly static Consumer Empty = new Consumer(UserId.Create(Guid.Empty), null, null, null, null, null);
    //TODO: Bargain object
    private List<OrderId> _orderIds = [];
    public IReadOnlyList<OrderId> OrderIds => _orderIds.AsReadOnly();

    public override UserType UserType => UserType.CONSUMER;

    private Consumer(UserId id, string fName, string lName, string email, string password, string mobileNo)
        : base(id, fName, lName, email, password, mobileNo)
    {
    }

    public static Consumer Create(string fName, string lName, string email, string password, string mobileNo)
    {
        return new(
                UserId.CreateUnique(),
                fName,
                lName,
                email,
                password,
                mobileNo);
    }

    /// <summary>
    /// Assigns the order id to the user
    /// </summary>
    /// <param name="orderId">Order Id to add</param>
    public void AddOrder(OrderId orderId)
    {
        _orderIds.Add(orderId);
    }

    //TODO: Change email, password, and mobile num. Maybe first and lastname

    private Consumer() { }
}

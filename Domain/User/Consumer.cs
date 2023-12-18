using Domain.Common;
using Domain.Order;

namespace Domain.User;

public class Consumer : Entity<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string MobileNo { get; private set; }

    //TODO: Bargain object
    private List<OrderId> _orderIds = new List<OrderId>();
    public IReadOnlyList<OrderId> OrderIds => _orderIds.AsReadOnly();
    private Consumer(UserId id, string fName, string lName, string email, string password, string mobileNo)
        : base(id)
    {
        FirstName = fName;
        LastName = lName;
        Email = email;
        Password = password;
        MobileNo = mobileNo;
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

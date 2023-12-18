using Domain.Common;
using Domain.Order;

namespace Domain.User;

public class Provider : Entity<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string MobileNo { get; private set; }
    private List<OrderId> _acceptedOrders = new();
    public IReadOnlyList<OrderId> AcceptedOrders => _acceptedOrders.AsReadOnly();


    private Provider(UserId id, string fName, string lName, string email, string password, string mobileNo)
        : base(id)
    {
        FirstName = fName;
        LastName = lName;
        Email = email;
        Password = password;
        MobileNo = mobileNo;
    }

    public static Provider Create(string fName, string lName, string email, string password, string mobileNo)
    {
        return new(
                UserId.CreateUnique(),
                fName,
                lName,
                email,
                password,
                mobileNo);
    }

    public void AssignOrder(OrderId orderId)
    {
        //TODO: Make sure the order also has ProviderId assigned to it. Can use domain events
        _acceptedOrders.Add(orderId);
    }
}

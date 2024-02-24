using Domain.Common;

namespace Domain.User;

public abstract class UserBase : Entity<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string MobileNo { get; private set; }
    public abstract UserType UserType { get; }
    public DateTime CreatedDate { get; private set; }

    protected UserBase(UserId id, string firstName, string lastName, string email, string password, string mobileNo)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        MobileNo = mobileNo;
        CreatedDate = DateTime.Now;
    }

    protected UserBase() { }
}

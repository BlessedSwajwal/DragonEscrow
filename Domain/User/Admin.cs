namespace Domain.User;
public class Admin : UserBase
{
    public override UserType UserType => UserType.ADMIN;
    public static Guid AdminId = new Guid("14b20e94-fa30-4a03-a5ed-8af619b149a0");
    public static Admin AdminInstance = new Admin();

    public Admin() : base(UserId.Create(AdminId), "Admin", "DealShield", "admin@dealshield.com", "F@kepassword1111", "0000000000")
    {

    }
}

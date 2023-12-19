using System.Net;

namespace Application.Common.Errors;

public class InvalidUserTypeError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => "Invalid Usertype. Must be either Consumer or Provider.";
}

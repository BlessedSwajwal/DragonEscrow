using System.Net;

namespace Application.Common.Errors;

public class UserAlreadyExistsError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => "Email already in use.";
}

using System.Net;

namespace Application.Common.Errors;

public class InvalidCredentialsError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.NotFound;

    public string? ErrorMessage => "No such user. Check credentials";
}

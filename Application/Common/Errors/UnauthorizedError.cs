using System.Net;

namespace Application.Common.Errors;

public class UnauthorizedError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.Forbidden;

    public string? ErrorMessage => "Not Authorized.";
}

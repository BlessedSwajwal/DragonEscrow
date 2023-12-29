using System.Net;

namespace Application.Common.Errors;

public class AlreadyBiddedError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => "Can only bid once in an order.";
}

using System.Net;

namespace Application.Common.Errors;

public class OrderNotFoundError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => "No such order.";
}

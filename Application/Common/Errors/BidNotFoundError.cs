using System.Net;

namespace Application.Common.Errors;

public class BidNotFoundError : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.NotFound;

    public string? ErrorMessage => "No such bid.";
}

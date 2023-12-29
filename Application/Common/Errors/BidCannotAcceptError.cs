using System.Net;

namespace Application.Common.Errors;

public class BidCannotAcceptError(string Status) : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => $"Can not accept bid. Order status: {Status}";

}

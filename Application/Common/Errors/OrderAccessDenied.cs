using System.Net;

namespace Application.Common.Errors;

public class OrderAccessDenied : IServiceError
{
    public int StatusCode => (int)HttpStatusCode.Unauthorized;

    public string? ErrorMessage => "Not Authorized to view the order details.";
}

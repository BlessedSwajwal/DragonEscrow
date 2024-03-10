using System.Net;

namespace Application.Common.Errors;

public class CustomError : IServiceError
{
    public HttpStatusCode ErrorCode { get; set; }
    public string? CustomMessage { get; set; }
    public int StatusCode => (int)ErrorCode;

    public string? ErrorMessage => CustomMessage;
}

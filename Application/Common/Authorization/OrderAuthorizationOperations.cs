using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Application.Common.Authorization;

public class OrderAuthorizationOperations
{
    public static class OrderOprations
    {
        public static OperationAuthorizationRequirement ViewPdf = new() { Name = Constants.ViewOrder };
    }
    public static class Constants
    {
        public static readonly string ViewOrder = "ViewOrder";
    }
}

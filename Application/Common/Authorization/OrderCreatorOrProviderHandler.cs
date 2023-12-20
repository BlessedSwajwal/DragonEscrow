using Domain.Order;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using static Application.Common.Authorization.OrderAuthorizationOperations;

namespace Application.Common.Authorization;

public class OrderCreatorOrProviderHandler : AuthorizationHandler<OperationAuthorizationRequirement, Order>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Order resource)
    {
        if (context.User is null) return Task.CompletedTask;

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Task.CompletedTask;
        }

        var userId = UserId.Create(Guid.Parse(userIdClaim.Value));

        if (requirement.Name != Constants.ViewOrder)
        {
            return Task.CompletedTask;
        }

        if (userId == resource.ConsumerId || context.User.FindFirst("UserType")!.Value.ToLower() == UserType.PROVIDER.ToString().ToLower())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

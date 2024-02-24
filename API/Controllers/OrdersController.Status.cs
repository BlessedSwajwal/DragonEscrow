using Application.Orders.Command.MarkFulfilled;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

public partial class OrdersController
{
    [HttpGet("MarkCompleted/{OrderId}")]
    public async Task<IActionResult> MarkCompleted([FromRoute] Guid OrderId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new MarkFulfilledCommand(userId, OrderId);
        var response = await _mediator.Send(command);
        return response.Match(
                someResponse => Ok("Done"),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }
}

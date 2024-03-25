using Application.Orders.Command.MarkDisputed;
using Application.Orders.Command.MarkFulfilled;
using Application.Orders.Command.RateOrder;
using Application.Orders.Command.VerifyOrderCompletion;
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

    [HttpGet("{OrderId}/rate")]
    public async Task<IActionResult> RateOrder([FromRoute] Guid OrderId, [FromQuery] int RatingCount)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new RateOrderCommand(userId, OrderId, RatingCount);
        var response = await _mediator.Send(command);
        return response.Match(
                someResponse => Ok("Done"),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("VerifyOrderCompletion/{OrderId}")]
    public async Task<IActionResult> CompleteOrder([FromRoute] Guid OrderId)
    {
        var consumerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new VerifyOrderCompletionCommand(consumerId, OrderId);
        var response = await _mediator.Send(command);
        return response.Match(
                someResponse => Ok(someResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("RaiseDispute/{OrderId}")]
    public async Task<IActionResult> RaiseDispute([FromRoute] Guid OrderId)
    {
        var consumerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = new MarkDisputedCommand(consumerId, OrderId);
        var response = await _mediator.Send(command);
        return response.Match(
                someResponse => Ok(someResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }
}

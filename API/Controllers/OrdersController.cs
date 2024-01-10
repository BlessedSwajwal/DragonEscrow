using Application.Orders.Command.AcceptOrder;
using Application.Orders.Command.CreateOrder;
using Application.Orders.Command.VerifyPayment;
using Application.Orders.Query.GetCreatedOrders;
using Application.Orders.Query.GetOrderDetail;
using Application.Users.Commands.AcceptBid;
using Contracts;
using Domain.User;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/Order")]
[ApiController]
public class OrdersController(ISender _mediator) : ControllerBase
{
    /// <summary>
    /// Only providers should call this endpoint. Gets all the order that has status 'CREATED' allowing 
    ///     providers to know what to bid on.
    /// </summary>
    /// <returns>List of orders with status 'Created' sorted with price.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCreatedOrders()
    {
        //Check if the user is actually provider. If not return problem unauthorized.
        if (User.FindFirstValue("UserType")! != UserType.PROVIDER.ToString().ToUpper())
            return Problem(detail: "Only providers can check all created orders.");

        var query = new GetCreatedOrderQuery();
        var response = await _mediator.Send(query);
        return response.Match(
                ordersResponse => Ok(ordersResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage)
                );
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        //Get userId
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var command = request.BuildAdapter<CreateOrderRequest>()
                        .AddParameters("ConsumerId", userId)
                        .AdaptToType<CreateOrderCommand>();

        var response = await _mediator.Send(command);
        return response.Match(
                orderResponse => Ok(orderResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> OrderDetail([FromRoute] Guid orderId)
    {
        var query = new GetOrderDetailQuery(User, orderId);
        var response = await _mediator.Send(query);
        return response.Match(
                orderResponse => Ok(orderResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("paymentCallback")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPayment([FromQuery] string pidx, [FromQuery] Guid purchase_order_id)
    {
        var command = new VerifyPaymentCommand(purchase_order_id, pidx);
        var response = await _mediator.Send(command);
        return response.Match(
                orderResponse => Ok(orderResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("Accept/{orderId}")]
    public async Task<IActionResult> AcceptOrder([FromRoute] Guid orderId)
    {
        if (User.FindFirstValue("UserType")! != UserType.PROVIDER.ToString().ToUpper())
            return Problem(detail: "Only providers can accept orders.");

        var command = new AcceptOrderCommand(orderId, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
        var response = await _mediator.Send(command);
        return response.Match(
                orderResponse => Ok(orderResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("{OrderId}/AcceptBid")]
    public async Task<IActionResult> AcceptBid([FromRoute] Guid OrderId, [FromQuery] Guid BidId)
    {
        if (BidId.Equals(Guid.Empty)) return Problem(detail: "BidId must be specified.");
        //if(Enum.Parse<UserType>(User.FindFirstValue("UserType")!) != UserType.CONSUMER)
        //{
        //    return Problem(detail: "Must be consumer to accept");
        //}
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new AcceptBidCommand(userId, OrderId, BidId);
        var response = await _mediator.Send(command);
        return response.Match(
                successResponse => Ok(successResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }
}

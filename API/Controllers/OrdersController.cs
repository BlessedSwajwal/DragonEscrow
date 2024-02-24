using Application.Common.Services;
using Application.Orders.Command.AcceptOrder;
using Application.Orders.Command.CreateOrder;
using Application.Orders.Command.VerifyBidPayment;
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
public partial class OrdersController(ISender _mediator, IPaymentService paymentService) : ControllerBase
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

    //[HttpGet("paymentCallback")]
    //[AllowAnonymous]
    //public async Task<IActionResult> VerifyPayment([FromQuery] string pidx, [FromQuery] Guid purchase_order_id)
    //{
    //    var command = new VerifyPaymentCommand(purchase_order_id, pidx);
    //    var response = await _mediator.Send(command);
    //    return response.Match(
    //            orderResponse => Ok(orderResponse),
    //            serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
    //            ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    //}

    [HttpGet("paymentCallback")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyBidPayment([FromQuery] string pidx, [FromQuery] Guid purchase_order_id, [FromQuery] Guid purchase_order_name)
    {
        //Purchase_order_name is bidId because bid was what was technically "purchased".
        var command = new VerifyBidPaymentCommand(pidx, purchase_order_id, purchase_order_name);
        var response = await _mediator.Send(command);
        return response.Match(
                orderResponse => Ok("Bid Accepted."),
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
    public async Task<IActionResult> AcceptBidWithKhalti([FromRoute] Guid OrderId, [FromQuery] Guid BidId)
    {
        if (BidId.Equals(Guid.Empty)) return Problem(detail: "BidId must be specified.");
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new AcceptBidCommand(userId, OrderId, BidId, "Khalti");
        var response = await _mediator.Send(command);
        return response.Match(
                paymentUriResponse => Ok(paymentUriResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("{OrderId}/AcceptBidWithStripe")]
    public async Task<IActionResult> AcceptBidWithStripe([FromRoute] Guid OrderId, [FromQuery] Guid BidId)
    {
        if (BidId.Equals(Guid.Empty)) return Problem(detail: "BidId must be specified.");

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new AcceptBidCommand(userId, OrderId, BidId, "Stripe");
        var response = await _mediator.Send(command);
        return response.Match(
                paymentUriResponse => Ok(paymentUriResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }


    [HttpPost("StripeWebHook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebHook()
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        await Console.Out.WriteLineAsync(body);
        await paymentService.AcceptBidAfterStripePayment(body, Request.Headers["Stripe-Signature"]!);
        return Ok();
    }
}

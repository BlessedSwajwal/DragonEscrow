using Application.Orders.Command.CreateOrder;
using Application.Orders.Command.VerifyPayment;
using Application.Orders.Query.GetOrderDetail;
using Contracts;
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

    [HttpGet("paymentCallback/{orderId}")]
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
}

using Application.Orders.Query.GetAllConsumerOrders;
using Application.Users.Commands.CreateConsumer;
using Application.Users.Query.LoginConsumer;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;


[ApiController]
[Route("api/Consumer")]
public class ConsumersController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ConsumersController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var command = new CreateConsumerCommand(request.FirstName, request.LastName, request.Email, request.Password, request.Phone);

        var response = await _mediator.Send(command);

        return response.Match(
                consumerResponse => Ok(consumerResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));

    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var query = _mapper.Map<LoginConsumerQuery>(request);

        var response = await _mediator.Send(query);
        return response.Match(
                consumerResponse => Ok(consumerResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("GetAllOrder")]
    public async Task<IActionResult> GetAllOrder()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetAllConsumerOrdersQuery(Guid.Parse(userId!));

        var response = await _mediator.Send(query);
        return response.Match(
                orderResponse => Ok(orderResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

}

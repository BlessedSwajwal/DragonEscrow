using Application.Users.Commands.AddBid;
using Application.Users.Commands.CreateProvider;
using Application.Users.Query.BidSelectedOrders;
using Application.Users.Query.LoginProvider;
using Application.Users.Query.ProducerDetail;
using Contracts;
using Domain.User;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/Provider")]
[ApiController]
public class ProvidersController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProvidersController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateProducer(CreateUserRequest request)
    {
        var command = new CreateProviderCommand(request.FirstName, request.LastName, request.Email, request.Password, request.Phone);

        var response = await _mediator.Send(command);

        return response.Match(
                userResponse => Ok(userResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));

    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginProvider(LoginUserRequest request)
    {
        var query = _mapper.Map<LoginProviderQuery>(request);
        var response = await _mediator.Send(query);

        return response.Match(
                userResponse => Ok(userResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpPost("CreateBid")]
    public async Task<IActionResult> CreateBid(CreateBidRequest request)
    {
        //User.FindFirstValue("UserType")!.ToLower() != UserType.PROVIDER.ToString().ToLower()
        if (!User.FindFirstValue("UserType").Equals(UserType.PROVIDER.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            return Problem(title: "Invalid User type", detail: $"User type: {User.FindFirstValue("UserType")} can not create bid.");
        }
        var command = request.BuildAdapter()
                            .AddParameters("ProviderId", User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                            .AdaptToType<AddBidCommand>();

        var response = await _mediator.Send(command);

        return response.Match(
                bidresponse => Ok(bidresponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [HttpGet("GetSelectedBids")]
    public async Task<IActionResult> GetAllSelectedBids()
    {
        var consumerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new BidsSelectedQuery(consumerId);

        var response = await _mediator.Send(query);

        return response.Match(
                bidresponse => Ok(bidresponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }


    [HttpGet("details")]
    public async Task<IActionResult> GetUserDetail()
    {
        var providerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new ProviderDetailQuery(providerId);
        var response = await _mediator.Send(query);
        return response.Match(
                providerResponse => Ok(providerResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }

    [AllowAnonymous]
    [HttpGet("providerDetails/{ProviderId}")]
    public async Task<IActionResult> GetPublicDetailOfProvider([FromRoute] Guid ProviderId)
    {
        Console.Out.WriteLine(ProviderId);
        var query = new ProviderDetailQuery(ProviderId);
        var response = await _mediator.Send(query);

        return response.Match(
                providerResponse => Ok(providerResponse),
                serviceError => Problem(title: "Error", statusCode: serviceError.StatusCode, detail: serviceError.ErrorMessage),
                ruleValidationErrors => Problem(title: "Error", statusCode: (int)HttpStatusCode.BadRequest, detail: ruleValidationErrors.GetValidationErrors()));
    }
}

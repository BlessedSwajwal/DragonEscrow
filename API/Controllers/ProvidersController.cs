using Application.Users.Commands.CreateProvider;
using Application.Users.Query.LoginProvider;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
}

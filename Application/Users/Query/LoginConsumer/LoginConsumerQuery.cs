using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Query.LoginConsumer;

public record LoginConsumerQuery(string Email, string Password) : IRequest<OneOf<UserResponse, IServiceError, ValidationErrors>>;

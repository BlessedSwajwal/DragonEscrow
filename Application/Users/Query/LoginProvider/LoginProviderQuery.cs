using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Query.LoginProvider;

public record LoginProviderQuery(string Email, string Password) : IRequest<OneOf<UserResponse, IServiceError, ValidationErrors>>;

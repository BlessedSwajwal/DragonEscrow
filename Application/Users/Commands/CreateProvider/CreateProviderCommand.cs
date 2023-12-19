using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Commands.CreateProvider;

public record CreateProviderCommand(string FirstName, string LastName, string Email, string Password, string Phone) : IRequest<OneOf<UserResponse, IServiceError, ValidationErrors>>;

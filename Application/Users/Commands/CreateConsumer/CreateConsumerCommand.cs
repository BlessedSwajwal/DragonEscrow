using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Commands.CreateConsumer;

public record CreateConsumerCommand(string FirstName, string LastName, string Email, string Password, string Phone) : IRequest<OneOf<UserResponse, IServiceError, ValidationErrors>>;

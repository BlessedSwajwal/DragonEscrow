using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Commands.CreateConsumer;

public class CreateConsumerCHandler : IRequestHandler<CreateConsumerCommand, OneOf<UserResponse, IServiceError, ValidationErrors>>
{
    public Task<OneOf<UserResponse, IServiceError, ValidationErrors>> Handle(CreateConsumerCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

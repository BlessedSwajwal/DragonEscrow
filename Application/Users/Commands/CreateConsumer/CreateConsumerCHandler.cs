using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using MediatR;
using OneOf;

namespace Application.Users.Commands.CreateConsumer;

public class CreateConsumerCHandler : IRequestHandler<CreateConsumerCommand, OneOf<UserResponse, IServiceError, ValidationErrors>>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtGenerator _jwtGenerator;

    public CreateConsumerCHandler(IUnitOfWork uow, IJwtGenerator jwtGenerator)
    {
        _uow = uow;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<OneOf<UserResponse, IServiceError, ValidationErrors>> Handle(CreateConsumerCommand request, CancellationToken cancellationToken)
    {
        //Check if user exists
        var consumer = await _uow.ConsumerRepository.GetByEmail(request.Email);

        if (!consumer.Equals(Consumer.Empty))
        {
            return new UserAlreadyExistsError();
        }

        //Hash password
        var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //Create consumer
        consumer = Consumer.Create(request.FirstName, request.LastName, request.Email, password, request.Phone);

        //Add Consumer
        await _uow.ConsumerRepository.AddConsumer(consumer);
        await _uow.SaveAsync();

        //Get token
        var token = _jwtGenerator.GenerateJwt(consumer);

        //Create user response
        var userResponse = new UserResponse(consumer.FirstName, consumer.LastName, consumer.Email, consumer.UserType.ToString(), token);

        return userResponse;
    }
}

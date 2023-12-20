using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using MediatR;
using OneOf;

namespace Application.Users.Query.LoginConsumer;

public class LoginConsumerQueryHandler : IRequestHandler<LoginConsumerQuery, OneOf<UserResponse, IServiceError, ValidationErrors>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginConsumerQueryHandler(IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<OneOf<UserResponse, IServiceError, ValidationErrors>> Handle(LoginConsumerQuery request, CancellationToken cancellationToken)
    {
        var consumer = await _unitOfWork.ConsumerRepository.GetByEmail(request.Email);
        if (consumer.Equals(Consumer.Empty))
            return new InvalidCredentialsError();

        //Verify password
        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, consumer.Password);

        if (!passwordCorrect)
            return new InvalidCredentialsError();

        //Generate token
        var token = _jwtGenerator.GenerateJwt(consumer);

        //Generate user response
        var userResponse = new UserResponse(consumer.Id.Value, consumer.FirstName, consumer.LastName, consumer.Email, consumer.MobileNo, consumer.UserType.ToString(), token);

        return userResponse;

    }
}

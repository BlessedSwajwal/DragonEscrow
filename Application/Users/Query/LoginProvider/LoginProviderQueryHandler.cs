using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using MediatR;
using OneOf;

namespace Application.Users.Query.LoginProvider;

public class LoginProviderQueryHandler(IUnitOfWork _unitOfWork, IJwtGenerator _jwtGenerator) : IRequestHandler<LoginProviderQuery, OneOf<UserResponse, IServiceError, ValidationErrors>>
{

    public async Task<OneOf<UserResponse, IServiceError, ValidationErrors>> Handle(LoginProviderQuery request, CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ProviderRepository.GetByEmail(request.Email);
        if (provider.Equals(Consumer.Empty))
            return new InvalidCredentialsError();

        //Verify password
        bool passwordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, provider.Password);

        if (!passwordCorrect)
            return new InvalidCredentialsError();

        //Generate token
        var token = _jwtGenerator.GenerateJwt(provider);

        //Generate user response
        var userResponse = new UserResponse(provider.Id.Value, provider.FirstName, provider.LastName, provider.Email, provider.MobileNo, provider.UserType.ToString(), token);

        return userResponse;
    }
}

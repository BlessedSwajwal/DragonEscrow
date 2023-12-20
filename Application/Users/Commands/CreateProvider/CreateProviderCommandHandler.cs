using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using MediatR;
using OneOf;

namespace Application.Users.Commands.CreateProvider;

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, OneOf<UserResponse, IServiceError, ValidationErrors>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;
    public CreateProviderCommandHandler(IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<OneOf<UserResponse, IServiceError, ValidationErrors>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        //Check if user exists
        var provider = await _unitOfWork.ProviderRepository.GetByEmail(request.Email);

        if (!provider.Equals(Consumer.Empty))
        {
            return new UserAlreadyExistsError();
        }

        //Hash password
        var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //Create consumer
        provider = Provider.Create(request.FirstName, request.LastName, request.Email, password, request.Phone);

        //Add Consumer
        await _unitOfWork.ProviderRepository.AddProvider(provider);
        await _unitOfWork.SaveAsync();

        //Get token
        var token = _jwtGenerator.GenerateJwt(provider);

        //Create user response
        var userResponse = new UserResponse(provider.Id.Value, provider.FirstName, provider.LastName, provider.Email, provider.MobileNo, provider.UserType.ToString(), token);

        return userResponse;
    }
}

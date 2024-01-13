using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Users.Query.ProducerDetail;

public class ProviderDetailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ProviderDetailQuery, OneOf<ProviderDetailResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<ProviderDetailResponse, IServiceError, ValidationErrors>> Handle(ProviderDetailQuery request, CancellationToken cancellationToken)
    {
        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(UserId.Create(request.ProviderId));

        if (provider.Equals(Consumer.Empty))
        {
            return new InvalidCredentialsError();
        }

        var providerResponse = provider.Adapt<ProviderDetailResponse>();
        return providerResponse;
    }
}

using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Users.Query.ConsumerDetail;

public class ConsumerDetailQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<ConsumerDetailQuery, OneOf<ConsumerDetailResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<ConsumerDetailResponse, IServiceError, ValidationErrors>> Handle(ConsumerDetailQuery request, CancellationToken cancellationToken)
    {
        var consumer = await unitOfWork.ConsumerRepository.GetByIdAsync(UserId.Create(request.ConsumerId));

        if (consumer.Equals(Consumer.Empty))
        {
            return new InvalidCredentialsError();
        }

        var consumerResponse = consumer.Adapt<ConsumerDetailResponse>();
        return consumerResponse;
    }
}

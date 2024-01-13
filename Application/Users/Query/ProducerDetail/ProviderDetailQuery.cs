using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Query.ProducerDetail;

public record ProviderDetailQuery(Guid ProviderId) : IRequest<OneOf<ProviderDetailResponse, IServiceError, ValidationErrors>>;


using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Users.Query.ConsumerDetail;

public record ConsumerDetailQuery(Guid ConsumerId) : IRequest<OneOf<ConsumerDetailResponse, IServiceError, ValidationErrors>>;

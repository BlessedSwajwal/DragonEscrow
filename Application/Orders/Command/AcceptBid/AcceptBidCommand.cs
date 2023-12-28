using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Users.Commands.AcceptBid;

public record AcceptBidCommand(Guid UserId, Guid OrderId, Guid BidId) : IRequest<OneOf<Success, IServiceError, ValidationErrors>>;

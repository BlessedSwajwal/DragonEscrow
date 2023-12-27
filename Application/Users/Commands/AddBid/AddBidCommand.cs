using Application.Common.Errors;
using Application.Orders;
using MediatR;
using OneOf;

namespace Application.Users.Commands.AddBid;

public record AddBidCommand(Guid ProviderId, Guid OrderId, int ProposedAmount, string Comment) : IRequest<OneOf<BidResponse, IServiceError, ValidationErrors>>;

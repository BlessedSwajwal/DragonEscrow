using Application.Common.Errors;
using Application.Orders.Command.AcceptBid;
using MediatR;
using OneOf;

namespace Application.Users.Commands.AcceptBid;

public record AcceptBidCommand(Guid UserId, Guid OrderId, Guid BidId, string PaymentMethod = "Khalti") : IRequest<OneOf<PaymentUriResponse, IServiceError, ValidationErrors>>;

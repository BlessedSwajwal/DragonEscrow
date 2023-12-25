using Application.Common.Errors;
using Application.Orders;
using MediatR;
using OneOf;

namespace Application.Users.Commands.AddBid;

public class AddBidCommandHandler : IRequestHandler<AddBidCommand, OneOf<BidResponse, IServiceError, ValidationErrors>>
{
    public Task<OneOf<BidResponse, IServiceError, ValidationErrors>> Handle(AddBidCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

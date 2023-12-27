using Application.Common.Authorization;
using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using OneOf;
using System.Security.Claims;

namespace Application.Orders.Query.GetOrderDetail;

public class GetOrderDetailQueryHandler(
    IAuthorizationService _authorizationService,
    IUnitOfWork unitOfWork,
    IPaymentService paymentUri) : IRequestHandler<GetOrderDetailQuery, OneOf<OrderResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<OrderResponse, IServiceError, ValidationErrors>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        //get the order
        Order order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));

        //Get creator of order

        if (order.Equals(Order.Empty)) return new OrderNotFoundError();

        //Check if the user is creator of the order or if the user is provider.
        var authResult = await _authorizationService.AuthorizeAsync(request.User, order, OrderAuthorizationOperations.OrderOprations.ViewPdf);

        if (!authResult.Succeeded)
        {
            return new OrderAccessDenied();
        }

        //Get user for payment bill
        object user = new
        {
            name = request.User.FindFirst(ClaimTypes.GivenName)!.Value + request.User.FindFirst(ClaimTypes.Name)!.Value,
            email = request.User.FindFirst(ClaimTypes.Email)!.Value,
            mobileNo = request.User.FindFirst(ClaimTypes.MobilePhone)!.Value
        };
        //Get Payment Uri
        var url = "";

        //Payment url only if the creator sent the request. Provider can also view the order but do not require payment Url.
        if (order.Status == OrderStatus.CANCELLED)
        {
            url = "Order has already been cancelled.";
        }
        else if (order.Status != OrderStatus.PENDING)
        {
            url = "Already paid.";
        }
        else if (request.User.FindFirst(ClaimTypes.NameIdentifier)!.Value == order.ConsumerId.Value.ToString())
        {
            url = await paymentUri.GetPaymentUriAsync(user, order);
        }

        //Get orders bid
        var bids = await unitOfWork.BidRepository.GetBidListAsync(order.BidIds);
        //Generate Bid Response
        var bidResponse = bids.Adapt<List<BidResponse>>();

        var orderResponse = order.BuildAdapter()
                                .AddParameters("PaymentUri", url)
                                .AddParameters("BidResponses", bidResponse)
                                .AdaptToType<OrderResponse>();

        return orderResponse;
    }
}

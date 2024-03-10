﻿using Domain.Order.ValueObjects;

namespace Domain.User;

public class Provider : UserBase
{
    public readonly static Provider Empty = new(UserId.Create(Guid.Empty), null, null, null, null, null);
    private List<OrderId> _acceptedOrders = new();
    public IReadOnlyList<OrderId> AcceptedOrders => _acceptedOrders.AsReadOnly();
    public double AvgRating { get; private set; }
    public int RatingCount { get; private set; }

    public override UserType UserType => UserType.PROVIDER;

    private Provider(UserId id, string fName, string lName, string email, string password, string mobileNo)
        : base(id, fName, lName, email, password, mobileNo)
    {
        AvgRating = 0;
        RatingCount = 0;
    }

    public static Provider Create(string fName, string lName, string email, string password, string mobileNo)
    {
        return new(
                UserId.CreateUnique(),
                fName,
                lName,
                email,
                password,
                mobileNo);
    }

    public void AssignOrder(OrderId orderId)
    {
        //TODO: Make sure the order also has ProviderId assigned to it. Can use domain events
        _acceptedOrders.Add(orderId);
    }

    public void AddRating(int rating)
    {
        RatingCount++;
        AvgRating = (AvgRating + rating) / RatingCount;
    }

    private Provider() { }
}

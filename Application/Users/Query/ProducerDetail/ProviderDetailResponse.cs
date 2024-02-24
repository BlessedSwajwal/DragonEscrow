namespace Application.Users.Query.ProducerDetail;

public record ProviderDetailResponse(Guid Id, string FirstName, string LastName, string Email, string MobileNo, string UserType, int TotalOrderCount, double AvgRating, int RatingCount);

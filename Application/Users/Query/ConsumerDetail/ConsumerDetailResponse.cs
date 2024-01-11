namespace Application.Users.Query.ConsumerDetail;

public record ConsumerDetailResponse(Guid Id, string FirstName, string LastName, string Email, string MobileNo, string UserType, int TotalOrderCount);

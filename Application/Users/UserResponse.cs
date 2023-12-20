namespace Application.Users;

public record UserResponse(Guid id, string FirstName, string LastName, string Email, string Phone, string UserType, string Token);

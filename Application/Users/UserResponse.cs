namespace Application.Users;

public record UserResponse(string FirstName, string LastName, string Email, string UserType, string Token);

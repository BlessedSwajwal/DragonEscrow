namespace Contracts;

public record CreateOrderRequest(string Name, string Description, int AllowedDays, int Cost);

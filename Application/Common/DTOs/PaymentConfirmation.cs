namespace Application.Common.DTOs;

public record PaymentConfirmation(string Pidx, int Total_amount, string Status, string TransactionId, int Fee, bool Refunded);

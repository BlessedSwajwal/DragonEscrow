using Application.Common.Services;
using Domain.Bids;
using Domain.Order;
using Domain.User;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    public readonly EmailSettings EmailSettings;

    public EmailSenderService(IOptions<EmailSettings> emailSettings)
    {
        EmailSettings = emailSettings.Value;
    }

    public async Task SendBidAcceptedEmail(Provider provider, Order order, Bid bid)
    {
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse(EmailSettings.From));
        email.Sender = new MailboxAddress(EmailSettings.DisplayName, EmailSettings.From);
        email.To.Add(MailboxAddress.Parse(provider.Email));

        email.Subject = "Bid accepted";
        email.Body = new TextPart(TextFormat.Plain) { Text = $"Congratulations. Your bid on the order titled '{order.Name}' has been accepted. Your proposed amount was: Nrs. {bid.ProposedAmount / 100}. Deadline: {order.DeadLine} UTC." };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(EmailSettings.Host, EmailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(EmailSettings.From, EmailSettings.AppPassword);
        await smtp.SendAsync(email);
        smtp.Dispose();
    }
}

using MimeKit;
// Eski System.Net.Mail ni o'chirib tashlang yoki uni alias orqali yashiring
using MailKitSmtpClient = MailKit.Net.Smtp.SmtpClient; 

namespace SmartWarehouse.Services;

public class EmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("lochinbekmajidov85@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        // Endi MailKitSmtpClient dan foydalanamiz
        using var smtp = new MailKitSmtpClient();
        
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        // EmailService.cs ichida
        await smtp.AuthenticateAsync("lochinbekmajidov3737@gmail.com", "11030506qwertyui");;
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
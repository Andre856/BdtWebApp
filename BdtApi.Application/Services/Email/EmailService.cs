using BdtShared.Dtos.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BdtApi.Application.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool SendEmail(EmailDto emailDto)
    {
        try
        {
            var user = _configuration.GetSection("EmailUsername").Value ??
            throw new Exception("Email config error.");
            var port = _configuration.GetSection("EmailPort").Value ??
                throw new Exception("Email config error.");
            var host = _configuration.GetSection("EmailHost").Value ??
                throw new Exception("Email config error.");
            var password = _configuration.GetSection("EmailPassword").Value ??
                throw new Exception("Email config error.");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(emailDto.To));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailDto.Body };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtp.Connect(host, int.Parse(port), SecureSocketOptions.Auto);
            smtp.Authenticate(user, password);
            smtp.Send(email);
            smtp.Disconnect(true);

            return true;
        }
        catch (Exception ex)
        {
            //Write to database which user failed to receive an email
            return false;
        }
    }
}

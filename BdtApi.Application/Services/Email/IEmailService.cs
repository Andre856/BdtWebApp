using BdtShared.Dtos.Emails;

namespace BdtApi.Application.Services.Email;

public interface IEmailService
{
    bool SendEmail(EmailDto emailDto);
}

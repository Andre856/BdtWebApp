using BdtShared.Dtos.Emails;

namespace BdtApi.Application.Services.Interfaces;

public interface IEmailService
{
    bool SendEmail(EmailDto emailDto);
}

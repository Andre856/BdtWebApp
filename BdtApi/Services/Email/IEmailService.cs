using BdtShared.Dtos.Emails;

namespace BDtApi.ApiServices.Email;

public interface IEmailService
{
    bool SendEmail(EmailDto emailDto);
}

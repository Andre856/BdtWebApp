using BdtDomain.Dtos.Emails;

namespace BdtApplication.ApiServices.Email;

public interface IEmailServiceApi
{
    bool SendEmail(EmailDto emailDto);
}

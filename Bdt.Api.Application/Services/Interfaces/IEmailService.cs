using Bdt.Shared.Dtos.Emails;

namespace Bdt.Api.Application.Services.Interfaces;

public interface IEmailService
{
    bool SendEmail(EmailDto emailDto);
}

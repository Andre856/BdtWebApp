using System.Security.Claims;

namespace Bdt.Client.AppServices.User;

public interface IUserService
{
    void SetUser(ClaimsPrincipal user);
    ClaimsPrincipal GetCurrentUser();
}

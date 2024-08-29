using System.Security.Claims;

namespace BdtApplication.AppServices.User;

public interface IUserService
{
    void SetUser(ClaimsPrincipal user);
    ClaimsPrincipal GetCurrentUser();
}

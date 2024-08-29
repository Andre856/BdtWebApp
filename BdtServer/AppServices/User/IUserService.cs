using System.Security.Claims;

namespace BdtServer.AppServices.User;

public interface IUserService
{
    void SetUser(ClaimsPrincipal user);
    ClaimsPrincipal GetCurrentUser();
}

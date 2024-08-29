using System.Security.Claims;

namespace BdtClient.AppServices.User;

public interface IUserService
{
    void SetUser(ClaimsPrincipal user);
    ClaimsPrincipal GetCurrentUser();
}

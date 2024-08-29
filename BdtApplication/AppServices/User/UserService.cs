using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace BdtApplication.AppServices.User;

public class UserService : IUserService
{
    private ClaimsPrincipal _currentUser = new(new ClaimsPrincipal());

    private AuthenticationStateProvider _authenticationStateProvider;
    private readonly IMemoryCache _cache;
    public UserService(IMemoryCache cache,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _cache = cache;
    }

    public ClaimsPrincipal GetCurrentUser()
    {
        return _currentUser;
    }

    public void SetUser(ClaimsPrincipal user)
    {
        var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (email is not null)
        {
            var accessToken = user.Claims.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
            //_userProvider.SetCurrentAccessToken(accessToken);
            //_userProvider.SetCurrentUser(user);
        }

        if (_currentUser != user)
        {
            _currentUser = user;
        }

        _currentUser = user;
    }
}

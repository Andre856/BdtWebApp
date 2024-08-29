using BdtShared.Models.App;

namespace BdtClient.AppServices.Session;
public class SessionService : ISessionService
{
    //private readonly IHttpContextAccessor _httpContextAccessor;
    private const string AccessTokenKey = "AccessToken";
    private const string RefreshTokenKey = "RefreshToken";
    private const string UserEmailKey = "UserEmail";
    private const string UserNameKey = "UserName";
    private const string UserIdKey = "UserId";
    private const string IsDarkModeKey = "IsDarkMode";
    private const string NotificationTimeKey = "NotificationTime";
    private const string IsFirstLoginKey = "IsFirstLogin";

    //public SessionService(IHttpContextAccessor httpContextAccessor)
    //{
    //    _httpContextAccessor = httpContextAccessor;
    //}
    public SessionService()
    {
    }

    public Task ClearSessionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetAccessTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRefreshTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserBasicDetail> GetUserDetailsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserEmailAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveAccessTokenAsync(string accessToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveUserDetailsAsync(UserBasicDetail userBasicDetail)
    {
        throw new NotImplementedException();
    }

    public Task SaveUserEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    //public Task SaveAccessTokenAsync(string accessToken)
    //{
    //    _httpContextAccessor.HttpContext.Session.SetString(AccessTokenKey, accessToken);
    //    return Task.CompletedTask;
    //}

    //public Task SaveRefreshTokenAsync(string refreshToken)
    //{
    //    _httpContextAccessor.HttpContext.Session.SetString(RefreshTokenKey, refreshToken);
    //    return Task.CompletedTask;
    //}

    //public Task<string> GetAccessTokenAsync()
    //{
    //    return Task.FromResult(_httpContextAccessor.HttpContext.Session.GetString(AccessTokenKey));
    //}

    //public Task<string> GetRefreshTokenAsync()
    //{
    //    return Task.FromResult(_httpContextAccessor.HttpContext.Session.GetString(RefreshTokenKey));
    //}

    //public Task SaveUserEmailAsync(string email)
    //{
    //    _httpContextAccessor.HttpContext.Session.SetString(UserEmailKey, email);
    //    return Task.CompletedTask;
    //}

    //public Task<string> GetUserEmailAsync()
    //{
    //    var email = _httpContextAccessor.HttpContext.Session.GetString(UserEmailKey);
    //    return Task.FromResult(email);
    //}

    //public Task ClearSessionAsync()
    //{
    //    _httpContextAccessor.HttpContext.Session.Clear();
    //    return Task.CompletedTask;
    //}

    //public Task SaveUserDetailsAsync(UserBasicDetail userBasicDetail)
    //{
    //    _httpContextAccessor.HttpContext.Session.SetString(UserEmailKey, userBasicDetail.Email);
    //    _httpContextAccessor.HttpContext.Session.SetString(UserNameKey, userBasicDetail.UserName);
    //    _httpContextAccessor.HttpContext.Session.SetString(AccessTokenKey, userBasicDetail.AccessToken);
    //    _httpContextAccessor.HttpContext.Session.SetString(RefreshTokenKey, userBasicDetail.RefreshToken);
    //    _httpContextAccessor.HttpContext.Session.SetString(UserIdKey, userBasicDetail.UserId);
    //    _httpContextAccessor.HttpContext.Session.SetString(IsDarkModeKey, userBasicDetail.IsDarkMode.ToString());
    //    _httpContextAccessor.HttpContext.Session.SetString(NotificationTimeKey, userBasicDetail.NotificationTime.ToString(@"hh\:mm\:ss"));
    //    _httpContextAccessor.HttpContext.Session.SetString(IsFirstLoginKey, userBasicDetail.IsFirstLogin.ToString());

    //    return Task.CompletedTask;
    //}

    //public Task<UserBasicDetail> GetUserDetailsAsync()
    //{
    //    var userDetail = new UserBasicDetail
    //    {
    //        AccessToken = _httpContextAccessor.HttpContext.Session.GetString(AccessTokenKey),
    //        Email = _httpContextAccessor.HttpContext.Session.GetString(UserEmailKey),
    //        UserName = _httpContextAccessor.HttpContext.Session.GetString(UserNameKey),
    //        RefreshToken = _httpContextAccessor.HttpContext.Session.GetString(RefreshTokenKey),
    //        UserId = _httpContextAccessor.HttpContext.Session.GetString(UserIdKey),
    //        IsDarkMode = bool.TryParse(_httpContextAccessor.HttpContext.Session.GetString(IsDarkModeKey), out bool isDark) ? isDark : false,
    //        NotificationTime = TimeSpan.TryParse(_httpContextAccessor.HttpContext.Session.GetString(NotificationTimeKey), out TimeSpan time) ? time : TimeSpan.Zero,
    //        IsFirstLogin = bool.TryParse(_httpContextAccessor.HttpContext.Session.GetString(IsFirstLoginKey), out bool isFirst) ? isFirst : false,
    //    };

    //    return Task.FromResult(userDetail);
    //}

    //Task<UserBasicDetail> ISessionService.GetUserDetailsAsync()
    //{
    //    throw new NotImplementedException();
    //}
}

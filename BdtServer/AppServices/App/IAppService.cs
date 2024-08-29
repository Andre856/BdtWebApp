using BdtShared.Dtos.Users;
using BdtShared.Models.App;

namespace BdtServer.AppServices.App;

public interface IAppService
{
    Task<ApiWrapper<bool>> SetFirstLoginToFalse();
    Task<ApiWrapper<bool>> ResendVerificationEmail(RegisterUserDto registerUserDto);
    Task<ApiWrapper<string>> ResetPassword(ResetPasswordModel resetPasswordModel);
    Task<ApiWrapper<string>> ForgotPassword(string email);
    Task<ApiWrapper<Tokens>> AuthenticateUser(AuthenticateUserDto authenticateUser);
    Task<ApiWrapper<string>> RegisterUser(RegisterUserDto registerUser);
    Task<bool> RefreshToken();
    Task<ApiWrapper<string>> UpdateUserNotificationTime(TimeSpan notificationTime);
    Task<ApiWrapper<string>> UpdateUserTheme(bool isDarkTheme);
    Task<ApiWrapper<bool>> CheckThemeIsDark();
    Task<bool> TokenIsValid();
    Task<ApiWrapper<bool>> DeleteUserAccount();
    Task LogoutUser();
    Task SaveUserDetailsAsync(UserBasicDetail userBasicDetail);
    Task<UserBasicDetail> GetUserDetailAsync();
}

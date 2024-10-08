﻿using Bdt.Shared.Dtos.Users;
using Bdt.Shared.Models.App;

namespace Bdt.Client.AppServices.App;

public interface IAppService
{
    Task<ApiWrapper<bool>> SetFirstLoginToFalse();
    Task<ApiWrapper<bool>> ResendVerificationEmail(RegisterUserDto registerUserDto);
    Task<ApiWrapper<string>> ResetPassword(ResetPasswordModel resetPasswordModel);
    Task<ApiWrapper<string>> ForgotPassword(string email);
    Task<ApiWrapper<Tokens>> AuthenticateUser(AuthenticateUserDto authenticateUser);
    Task<ApiWrapper<string>> RegisterUser(RegisterUserDto registerUser);
    Task<bool> RefreshTokenAsync();
    Task<ApiWrapper<string>> UpdateUserNotificationTime(TimeSpan notificationTime);
    Task<ApiWrapper<string>> UpdateUserThemeAsync(bool isDarkTheme);
    Task<ApiWrapper<bool>> CheckThemeIsDark();
    Task<bool> TokenIsValid();
    Task<ApiWrapper<bool>> DeleteUserAccount();
}

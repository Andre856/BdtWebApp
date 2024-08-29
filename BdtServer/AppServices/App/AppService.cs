using BdtServer.AppServices.Session;
using BdtShared.Dtos.Users;
using BdtShared.Models.App;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BdtServer.AppServices.App;

public class AppService : IAppService
{
    private readonly ISessionService _sessionService;
    private readonly IHttpClientFactory _httpClientFactory;
    public AppService(IHttpClientFactory httpClientFactory, ISessionService sessionService)
    {
        _sessionService = sessionService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiWrapper<bool>> SetFirstLoginToFalse()
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");
        var tokenIsValid = await TokenIsValid();

        if (!tokenIsValid)
            throw new UnauthorizedAccessException("The token could not be refreshed");

        var setFirstLoginDto = new SetFirstLoginDto
        {
            Email = await _sessionService.GetUserEmailAsync() // Replace Setting.UserBasicDetail.Email with session service
        };

        var serialisedString = JsonConvert.SerializeObject(setFirstLoginDto);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/SetFirstLoginToFalse", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<bool>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<bool>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<bool>> ResendVerificationEmail(RegisterUserDto registerUserDto)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var serialisedString = JsonConvert.SerializeObject(registerUserDto);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/ResendVerificationEmail", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            return ApiWrapper<bool>.Success(content.Data);
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<bool>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<bool>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<string>> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var serialisedString = JsonConvert.SerializeObject(resetPasswordModel);

        var response = await httpClient.PostAsync("v1/Users/ResetPassword", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            string successString = await response.Content.ReadAsStringAsync();

            return ApiWrapper<string>.Success(successString);
        }

        return ApiWrapper<string>.Failed("Failed to reset password.");
    }

    public async Task<ApiWrapper<string>> ForgotPassword(string email)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var forgotPasswordModel = new ForgotPasswordModel
        {
            Email = email
        };

        var serialisedString = JsonConvert.SerializeObject(forgotPasswordModel);

        var response = await httpClient.PostAsync("v1/Users/ForgotPassword", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            string resetPasswordToken = await response.Content.ReadAsStringAsync();

            return ApiWrapper<string>.Success(resetPasswordToken);
        }

        return ApiWrapper<string>.Failed("Failed to reset password.");
    }

    public async Task<ApiWrapper<string>> UpdateUserNotificationTime(TimeSpan notificationTime)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var updateUserNotificationTimeDto = new UpdateUserNotificationTimeDto
        {
            Email = await _sessionService.GetUserEmailAsync(), // Replace Setting.UserBasicDetail.Email with session service
            NotificationTime = notificationTime
        };

        var serialisedString = JsonConvert.SerializeObject(updateUserNotificationTimeDto);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/UpdateUserNotificationTime", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();

            return ApiWrapper<string>.Success(contentString);
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<string>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<string>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<bool>> CheckThemeIsDark()
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");

        var getUserThemeDto = new GetUserThemeDto
        {
            Email = await _sessionService.GetUserEmailAsync() // Replace Setting.UserBasicDetail.Email with session service
        };

        var serialisedString = JsonConvert.SerializeObject(getUserThemeDto);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/CheckThemeIsDark", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<bool>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<bool>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<string>> UpdateUserTheme(bool isDarkTheme)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var updateUserThemeDto = new UpdateUserThemeDto
        {
            Email = await _sessionService.GetUserEmailAsync(), // Replace Setting.UserBasicDetail.Email with session service
            IsDarkTheme = isDarkTheme
        };

        var serialisedString = JsonConvert.SerializeObject(updateUserThemeDto);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/UpdateUserTheme", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<string>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<string>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<string>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<Tokens>> AuthenticateUser(AuthenticateUserDto authenticateUser)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var serialisedString = JsonConvert.SerializeObject(authenticateUser);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/Authenticate", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<Tokens>>(contentString);

            if (content.IsSuccess)
            {
                return ApiWrapper<Tokens>.Success(content.Data);
            }

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<Tokens>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<Tokens>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<ApiWrapper<string>> RegisterUser(RegisterUserDto registerUser)
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        var serialisedString = JsonConvert.SerializeObject(registerUser);

        try
        {
            var response = await httpClient.PostAsync("v1/Users/RegisterUser", new StringContent(serialisedString, Encoding.UTF8, "application/json"));

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<string>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<string>.Failed($"Request error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<string>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task<bool> RefreshToken()
    {
        throw new NotImplementedException();
        //var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        //bool isTokenRefreshed = false;

        ////var accessToken = _tokenProvider.GetAccessToken();
        //var refreshToken = await _sessionService.GetRefreshTokenAsync();

        //var serializedString = JsonConvert.SerializeObject(new Tokens
        //{
        //    RefreshToken = refreshToken,
        //    AccessToken = accessToken
        //});

        //try
        //{
        //    var response = await httpClient.PostAsync("v1/Users/RefreshToken", new StringContent(serializedString, Encoding.UTF8, "application/json"));

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string contentString = await response.Content.ReadAsStringAsync();
        //        var mainResponse = JsonConvert.DeserializeObject<ApiWrapper<Tokens>>(contentString);

        //        if (mainResponse.IsSuccess)
        //        {
        //            await _sessionService.SaveAccessTokenAsync(mainResponse.Data.AccessToken);
        //            await _sessionService.SaveRefreshTokenAsync(mainResponse.Data.RefreshToken);

        //            isTokenRefreshed = true;
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string msg = ex.Message;
        //    // Log message if needed
        //}

        //return isTokenRefreshed;
    }

    public async Task<bool> TokenIsValid()
    {
        throw new NotImplementedException();
        //var accessToken = _tokenProvider.GetAccessToken();
        //var handler = new JwtSecurityTokenHandler();
        //var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;
        //bool tokenIsValid = jsonToken.ValidTo > DateTime.Now;

        //if (!tokenIsValid)
        //{
        //    var isTokenRefreshed = await RefreshToken();
        //    tokenIsValid = isTokenRefreshed;
        //}

        //return tokenIsValid;
    }

    public async Task<ApiWrapper<bool>> DeleteUserAccount()
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");
        var tokenIsValid = await TokenIsValid();

        if (!tokenIsValid)
            throw new UnauthorizedAccessException("The token could not be refreshed");

        var accessToken = await _sessionService.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var response = await httpClient.DeleteAsync("v1/Users/DeleteUserAccount");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            return content;
        }
        catch (HttpRequestException ex)
        {
            return ApiWrapper<bool>.Failed($"Delete error. Exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiWrapper<bool>.Failed($"Api call failed. Exception: {ex.Message}");
        }
    }

    public async Task LogoutUser()
    {
        await _sessionService.ClearSessionAsync();
        // Further logout handling, e.g., redirect to login page or show logout confirmation
    }

    public async Task SaveUserDetailsAsync(UserBasicDetail userBasicDetail)
    {
        await _sessionService.SaveUserDetailsAsync(userBasicDetail);
    }

    public async Task<UserBasicDetail> GetUserDetailAsync()
    {
        // Retrieve user details from the session
        var userBasicDetail = await _sessionService.GetUserDetailsAsync();

        return userBasicDetail;
    }
}

using Bdt.Shared.Dtos.Users;
using Bdt.Shared.Models.App;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Bdt.Client.Provider;
using Bdt.Client.Provider.Token;
using Bdt.Client.AppServices.Claims;

namespace Bdt.Client.AppServices.App;

public class AppService : IAppService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IClaimService _claimService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly CustomAuthStateProvider _authStateProvider;
    public AppService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider, ILogger<AppService> logger, CustomAuthStateProvider authStateProvider, IClaimService claimService)
    {
        _tokenProvider = tokenProvider;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _authStateProvider = authStateProvider;
        _claimService = claimService;
    }

    public async Task<ApiWrapper<bool>> SetFirstLoginToFalse()
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");
        var tokenIsValid = await TokenIsValid();

        if (!tokenIsValid)
            throw new UnauthorizedAccessException("The token could not be refreshed");

        var setFirstLoginDto = new SetFirstLoginDto
        {
            Email = await _claimService.GetEmailAsync()
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/Users/SetFirstLoginToFalse", setFirstLoginDto);

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            if (content.IsSuccess)
            {
                await RefreshTokenAsync();
            }

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
            Email = await _claimService.GetEmailAsync(),
            NotificationTime = notificationTime
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/Users/UpdateUserNotificationTime", updateUserNotificationTimeDto);

            string contentString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await RefreshTokenAsync();
            }

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
            Email = string.Empty//await _sessionService.GetUserEmailAsync() // Replace Setting.UserBasicDetail.Email with session service
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

    public async Task<ApiWrapper<string>> UpdateUserThemeAsync(bool isDarkTheme)
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");
        var updateUserThemeDto = new UpdateUserThemeDto
        {
            Email = await _claimService.GetEmailAsync(),
            IsDarkTheme = isDarkTheme
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/Users/UpdateUserTheme", updateUserThemeDto);

            string contentString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await RefreshTokenAsync();
            }

            return ApiWrapper<string>.Success(contentString); ;
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

        try
        {
            var response = await httpClient.PostAsJsonAsync("v1/Users/Authenticate", authenticateUser);

            // Deserialize the response content
            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<Tokens>>(contentString);

            if (content?.IsSuccess == true && content.Data is not null)
            {
                // If authentication is successful, update the authentication state
                await _authStateProvider.MarkUserAsAuthenticated(content.Data);
                return ApiWrapper<Tokens>.Success(content.Data);
            }

            // If authentication fails, ensure the user is logged out and return the failed content
            await _authStateProvider.MarkUserAsLoggedOutAsync();
            return content ?? ApiWrapper<Tokens>.Failed("Authentication failed with no response content.");
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

        try
        {
            // Use PostAsJsonAsync to simplify serialization and sending of the request
            var response = await httpClient.PostAsJsonAsync("v1/Users/RegisterUser", registerUser);

            // Read and deserialize the response content
            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<string>>(contentString);

            // Return the deserialized content or a failure if deserialization failed
            return content ?? ApiWrapper<string>.Failed("Failed to deserialize server response.");
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

    public async Task<bool> RefreshTokenAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("NoAuthBdtApi");
        bool isTokenRefreshed = false;

        var accessToken = await _tokenProvider.GetAccessTokenAsync();
        var refreshToken = await _tokenProvider.GetRefreshTokenAsync();

        var serializedString = JsonConvert.SerializeObject(new Tokens
        {
            RefreshToken = refreshToken,
            AccessToken = accessToken
        });

        try
        {
            var response = await httpClient.PostAsync("v1/Users/RefreshToken", new StringContent(serializedString, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string contentString = await response.Content.ReadAsStringAsync();
                var mainResponse = JsonConvert.DeserializeObject<ApiWrapper<Tokens>>(contentString);

                if (mainResponse.IsSuccess)
                {
                    await _authStateProvider.MarkUserAsAuthenticated(mainResponse.Data);

                    isTokenRefreshed = true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while refreshing the token.");
        }

        return isTokenRefreshed;
    }

    public async Task<bool> TokenIsValid()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();

        return authState.User.Identity.IsAuthenticated;
    }

    public async Task<ApiWrapper<bool>> DeleteUserAccount()
    {
        var httpClient = _httpClientFactory.CreateClient("BdtApi");
        var tokenIsValid = await TokenIsValid();

        if (!tokenIsValid)
            throw new UnauthorizedAccessException("The token could not be refreshed");

        var accessToken = await _tokenProvider.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var response = await httpClient.DeleteAsync("v1/Users/DeleteUserAccount");

            string contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ApiWrapper<bool>>(contentString);

            await _authStateProvider.MarkUserAsLoggedOutAsync();

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
}

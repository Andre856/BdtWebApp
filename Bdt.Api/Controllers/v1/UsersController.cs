using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Context;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos.Emails;
using Bdt.Shared.Dtos.Users;
using Bdt.Shared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bdt.Api.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IServer _server;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly BdtDbContext _context;

    private readonly string _emailHtmlPath;
    private const string PROVIDER = "Busy Dad Training";
    private const string VERIFICATIONTOKEN = "VerificationToken";
    private const string RESETPASSWORD = "ResetPassword";
    public UsersController(IMapper mapper, IConfiguration configuration, IEmailService emailService, IServer server,
        UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, BdtDbContext context)
    {
        _mapper = mapper;
        _userManager = userManager;
        _emailService = emailService;
        _server = server;
        _roleManager = roleManager;
        _configuration = configuration;
        _emailHtmlPath = $"{AppDomain.CurrentDomain.BaseDirectory}EmailTemplate\\EmailConfirm.html";
        _context = context;
    }

    [HttpPost("SetFirstLoginToTrue")]
    public async Task<ActionResult<ApiWrapper<bool>>> SetFirstLoginToTrue(SetFirstLoginDto setFirstLoginDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(setFirstLoginDto.Email);

        if (userDetails is not null)
        {
            userDetails.IsFirstLogin = true;
            await _userManager.UpdateAsync(userDetails);

            return Ok(ApiWrapper<bool>.Success(userDetails.IsFirstLogin));
        }

        return BadRequest(ApiWrapper<bool>.Failed("No user was found for this email."));
    }

    [HttpPost("SetFirstLoginToFalse")]
    public async Task<ActionResult<ApiWrapper<bool>>> SetFirstLoginToFalse(SetFirstLoginDto setFirstLoginDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(setFirstLoginDto.Email);

        if (userDetails is not null)
        {
            userDetails.IsFirstLogin = false;
            await _userManager.UpdateAsync(userDetails);

            return Ok(ApiWrapper<bool>.Success(userDetails.IsFirstLogin));
        }

        return BadRequest(ApiWrapper<bool>.Failed("No user was found for this email."));
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(DeleteUserDto deleteUserDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(deleteUserDto.Email);

        if (existingUser is not null)
        {
            var response = await _userManager.DeleteAsync(existingUser);

            if (response.Succeeded)
                return Ok("User has been deleted.");

            return BadRequest(response.Errors);
        }

        return BadRequest("No user was found for this email.");
    }

    [HttpDelete("DeleteUserAccount")]
    public async Task<ActionResult<ApiWrapper<bool>>> DeleteUserAccount()
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            if (existingUser is not null)
            {

                var userSubscription = await _context.Subscriptions.Where(x => x.UserId == userId).ToListAsync();
                var stripeCustomers = await _context.StripeCustomer.Where(x => x.UserId == userId).ToListAsync();
                var userPlans = await _context.Planner.Where(x => x.UserId == userId).ToListAsync();
                var userWorkouts = await _context.Workout.Where(x => x.UserId == userId).ToListAsync();
                var userWorkoutValues = await _context.WorkoutValues.Where(x => userWorkouts.Select(x => x.Id).Contains(x.WorkoutId)).ToListAsync();

                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.WorkoutValues.RemoveRange(userWorkoutValues);
                    _context.Workout.RemoveRange(userWorkouts);
                    await _context.SaveChangesAsync();

                    _context.Subscriptions.RemoveRange(userSubscription);
                    _context.Planner.RemoveRange(userPlans);
                    await _context.SaveChangesAsync();

                    _context.StripeCustomer.RemoveRange(stripeCustomers);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw;
                }

                var userDeleted = await _userManager.DeleteAsync(existingUser);

                if (userDeleted.Succeeded)
                    return Ok(ApiWrapper<bool>.Success(true));

                return BadRequest(ApiWrapper<bool>.Failed(userDeleted.Errors.ToString()));
            }

            return BadRequest(ApiWrapper<bool>.Failed("No user was found for this email."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiWrapper<bool>.Failed($"There was an error: {ex.Message}"));
        }
    }

    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole(UserRoleDto userRoleDto)
    {
        var roleToBeAdded = _mapper.Map<IdentityRole>(userRoleDto);

        var response = await _roleManager.CreateAsync(roleToBeAdded);

        if (!response.Succeeded)
        {
            return BadRequest(response.Errors);
        }

        return Ok("New role successfully created.");
    }

    [HttpDelete("DeleteRole")]
    public async Task<IActionResult> DeleteRole(UserRoleDto deleteRoleDto)
    {
        var existingRole = await _roleManager.FindByNameAsync(deleteRoleDto.RoleName);

        if (existingRole is not null)
        {
            var response = await _roleManager.DeleteAsync(existingRole);

            if (response.Succeeded)
                return Ok("User has been deleted.");

            return BadRequest(response.Errors);
        }

        return BadRequest("No user was found for this email.");
    }

    [HttpPost("AssignRoleToUser")]
    public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserDto assignRoleToUserDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(assignRoleToUserDto.Email);

        if (userDetails is not null)
        {
            var userRoleAssignResponse = await _userManager.AddToRoleAsync(userDetails, assignRoleToUserDto.RoleName);

            if (!userRoleAssignResponse.Succeeded)
                return BadRequest(userRoleAssignResponse.Errors);

            return Ok($"Role: {assignRoleToUserDto.RoleName} has been assigned to user: {assignRoleToUserDto.Email}");
        }

        return BadRequest("No user was found for this email.");
    }

    [HttpPost("UpdateUserNotificationTime")]
    public async Task<IActionResult> UpdateUserNotificationTime(UpdateUserNotificationTimeDto updateUserNotificationTimeDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(updateUserNotificationTimeDto.Email);

        if (userDetails is not null)
        {
            userDetails.NotificationTime = updateUserNotificationTimeDto.NotificationTime;
            await _userManager.UpdateAsync(userDetails);

            return Ok("User notification time has been updated");
        }

        return BadRequest("No user was found for this email.");
    }

    [HttpPost("UpdateUserTheme")]
    public async Task<IActionResult> UpdateUserTheme(UpdateUserThemeDto updateUserThemeDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(updateUserThemeDto.Email);

        if (userDetails is not null)
        {
            userDetails.IsDarkTheme = updateUserThemeDto.IsDarkTheme;
            await _userManager.UpdateAsync(userDetails);

            return Ok("User Theme has been updated");
        }

        return BadRequest("No user was found for this email.");
    }

    [HttpPost("CheckThemeIsDark")]
    public async Task<ActionResult<ApiWrapper<bool>>> CheckThemeIsDark(GetUserThemeDto getUserThemeDto)
    {
        var userDetails = await _userManager.FindByEmailAsync(getUserThemeDto.Email);

        if (userDetails is not null)
        {
            await _userManager.UpdateAsync(userDetails);

            return Ok(ApiWrapper<bool>.Success(userDetails.IsDarkTheme));
        }

        return BadRequest(ApiWrapper<bool>.Failed("No user was found for this email."));
    }

    [AllowAnonymous]
    [HttpPost("RegisterUser")]
    public async Task<ActionResult<ApiWrapper<bool>>> RegisterUser(RegisterUserDto registerUserDto)
    {
        try
        {
            var serverAddressesFeature = _server.Features.Get<IServerAddressesFeature>();
            var thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();

            var user = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (user is not null)
                return BadRequest(ApiWrapper<bool>.Failed("This email already exists."));

            var userToBeCreated = _mapper.Map<UserEntity>(registerUserDto);

            var response = await _userManager.CreateAsync(userToBeCreated, registerUserDto.Password);

            if (!response.Succeeded)
                return BadRequest(ApiWrapper<bool>.Failed("Could not register user."));

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userToBeCreated);
            if (string.IsNullOrEmpty(emailConfirmationToken))
                return BadRequest("Error.");

            await _userManager.SetAuthenticationTokenAsync(userToBeCreated, PROVIDER, VERIFICATIONTOKEN, emailConfirmationToken);

            thisApiUrl = "https://bdtapi-ghgqdkfbdxb2etgb.eastus-01.azurewebsites.net";
            var link = $"{thisApiUrl}/api/v1/Users/VerifyUser?uid={userToBeCreated.Id}&token={emailConfirmationToken}";

            //using var reader = new StreamReader(_emailHtmlPath);
            //string emailHtml = reader.ReadToEnd();

            var emailHtml = $"""
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset="utf-8" />
                    <title></title>
                </head>
                <body>
                    <img src="http://drive.google.com/uc?export=view&id=1p9IgKdAEsL8Ct8Vuf23zqjUsiyYZYa" alt="Busy Dad Training" />

                    <p>Hi {userToBeCreated.UserName}!,</p>
                    <br />
                    <p>
                        Thank you for signing up to Busy Dad Training!
                        <br />
                        You have created a new account on the Busy Dad Training App.
                        <br />
                        Please click the following linke to verify your email address.
                        <br />
                        <br />
                        <a href="{link}">Verify email</a>
                    </p>
                </body>
                </html>
                """;
            //emailHtml = emailHtml.Replace("{{UserName}}", userToBeCreated.UserName);
            //emailHtml = emailHtml.Replace("{{Link}}", link);

            var emailDto = new EmailDto
            {
                To = userToBeCreated.Email,
                Subject = "Busy Dad Training Email Confirmation",
                Body = emailHtml
            };

            var emailSent = _emailService.SendEmail(emailDto);

            if (emailSent)
                return Ok(ApiWrapper<bool>.Success(true));

            return BadRequest(ApiWrapper<string>.Failed("Could not register user."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiWrapper<string>.Failed(ex.Message));
        }
    }

    [AllowAnonymous]
    [HttpPost("ResendVerificationEmail")]
    public async Task<ActionResult<ApiWrapper<bool>>> ResendVerificationEmail(RegisterUserDto registerUserDto)
    {
        var serverAddressesFeature = _server.Features.Get<IServerAddressesFeature>();
        var thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();

        var user = await _userManager.FindByEmailAsync(registerUserDto.Email);
        if (user is null)
            return BadRequest(ApiWrapper<bool>.Failed("User does not exist."));

        var verificationToken = await _userManager.GetAuthenticationTokenAsync(user, PROVIDER, VERIFICATIONTOKEN);

        if (string.IsNullOrEmpty(verificationToken))
        {
            verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (string.IsNullOrEmpty(verificationToken))
                return BadRequest("Error.");

            await _userManager.SetAuthenticationTokenAsync(user, PROVIDER, VERIFICATIONTOKEN, verificationToken);
        }

        var link = $"{thisApiUrl}/api/v1/Users/VerifyUser?uid={user.Id}&token={verificationToken}";

        using var reader = new StreamReader(_emailHtmlPath);
        string emailHtml = reader.ReadToEnd();
        emailHtml.Replace("{{UserName}}", user.UserName);
        emailHtml.Replace("{{Link}}", link);

        var emailDto = new EmailDto
        {
            To = user.Email,
            Subject = "Busy Dad Training Email Confirmation",
            Body = emailHtml
        };

        var emailSent = _emailService.SendEmail(emailDto);

        if (emailSent)
            return Ok(ApiWrapper<bool>.Success(true));

        return BadRequest(ApiWrapper<string>.Failed("Could not send email."));
    }

    [AllowAnonymous]
    [HttpGet("VerifyUser")]
    public async Task<IActionResult> VerifyUser(string uid, string token)
    {
        var user = await _userManager.FindByIdAsync(uid);

        if (user is null)
            return BadRequest("Token could not be found.");

        var result = await _userManager.ConfirmEmailAsync(user, token.Replace(" ", "+"));

        string imageUrl = "https://purple-river-0804f4b0f.5.azurestaticapps.net/images/busy_dad_black_logo_medium.png";

        if (result.Succeeded)
        {
            var htmlContent = $"""
                <html>
                <body style="width: 80%; margin: 0 auto; text-align: center;">
                    <img src="{imageUrl}" alt="Verification Successful" style="width: 50%; height: auto; display: block; margin: 0 auto;">
                    <h2>Your email has been verified.</h2>
                    <p>Please continue to the login screen to complete your registration.</p>
                    <br>
                    <a href="https://purple-river-0804f4b0f.5.azurestaticapps.net/">Busy Dad Login</a>
                </body>
                </html>
                """;

            return new ContentResult
            {
                Content = htmlContent,
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        return BadRequest("Failed to verify user.");
    }

    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (user is null)
                return BadRequest("User could not be found.");

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(resetPasswordToken))
                return BadRequest("Error generating reset token.");

            // URL Encode the token
            var encodedToken = HttpUtility.UrlEncode(resetPasswordToken);

            // Generate a URL with the token for the user to reset their password
            //var websiteBaseUrl = "https://localhost:7114";
            var websiteBaseUrl = "https://purple-river-0804f4b0f.5.azurestaticapps.net";
            var resetPasswordUrl = $"{websiteBaseUrl}/forgotpassword/{user.Email}/{encodedToken}";

            // Prepare email content
            var emailHtml = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8" />
                <title>Reset Your Password</title>
            </head>
            <body>
                <img src="{websiteBaseUrl}/images/busy_dad_black_logo_medium.png" alt="Busy Dad Training" />
                <p>Hi {user.UserName},</p>
                <p>You requested to reset your password. Click the link below to reset it:</p>
                <a href="{resetPasswordUrl}">Reset Password</a>
                <p>If you did not request this, please ignore this email.</p>
            </body>
            </html>
        """;

            // Create and send the email
            var emailDto = new EmailDto
            {
                To = user.Email,
                Subject = "Reset Password Request",
                Body = emailHtml
            };

            var emailSent = _emailService.SendEmail(emailDto);

            if (emailSent)
                return Ok("Reset password link sent to your email.");

            return BadRequest("Error sending reset password email.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Could not send reset password link: {ex.Message}");
        }
    }

    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

        if (user is null)
            return BadRequest("User could not be found.");

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

        if (result.Succeeded)
            return Ok("Password successfully reset.");

        return BadRequest("Password reset failed.");
    }

    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public async Task<ActionResult<ApiWrapper<Tokens>>> AuthenticateUser(AuthenticateUserDto authenticateUserDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(authenticateUserDto.Email);

            if (user is null)
                return Unauthorized(ApiWrapper<Tokens>.Failed("Email not registered"));

            if (!user.EmailConfirmed)
            {
                var resenddto = new RegisterUserDto
                {
                    Email = authenticateUserDto.Email,
                    Password = authenticateUserDto.Password,
                    UserName = ""
                };

                var resp = await ResendVerificationEmail(resenddto);

                var res = resp.Result as OkObjectResult;
                var x = res.Value as ApiWrapper<bool>;
                if (x.IsSuccess)
                    return Unauthorized(ApiWrapper<Tokens>.Failed("Email not authorized."));
                else
                    return BadRequest(ApiWrapper<Tokens>.Failed("Failed to send verification email."));
            }


            bool isValidUser = await _userManager.CheckPasswordAsync(user, authenticateUserDto.Password);

            if (isValidUser)
            {
                string accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                var data = new Tokens
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return Ok(ApiWrapper<Tokens>.Success(data));
            }

            return Unauthorized(ApiWrapper<Tokens>.Failed("Incorrect email or password"));
        }
        catch (Exception ex)
        {
            return Unauthorized(ApiWrapper<Tokens>.Failed($"The following error occurred during Authorization. \nException: {ex.Message}"));
        }
    }

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<ApiWrapper<Tokens>>> RefreshToken(Tokens refreshTokenRequest)
    {
        if (refreshTokenRequest == null)
            return BadRequest(ApiWrapper<Tokens>.Failed("No tokens provided."));

        var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);

        if (principal is not null)
        {
            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email.Value);

            if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken || !user.EmailConfirmed)
                return BadRequest(ApiWrapper<Tokens>.Failed("Invalid Request"));

            string newAccessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            var requestAndResponse = new Tokens
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            };

            return Ok(ApiWrapper<Tokens>.Success(requestAndResponse));
        }

        return BadRequest(ApiWrapper<Tokens>.Failed("Invalid token found."));
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var keyDetail = Encoding.UTF8.GetBytes(_configuration["JwtKey"]);
        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["JwtIssuer"],
            ValidAudience = _configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyDetail),
        };

        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    private string GenerateAccessToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var keyDetail = Encoding.UTF8.GetBytes(_configuration["JwtKey"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("IsDarkTheme", user.IsDarkTheme.ToString()),
            new Claim("NotificationTime", user.NotificationTime.ToString()),
            new Claim("IsFirstLogin", user.IsFirstLogin.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["JwtAudience"],
            Issuer = _configuration["JwtIssuer"],
            Expires = DateTime.UtcNow.AddMinutes(120),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyDetail), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

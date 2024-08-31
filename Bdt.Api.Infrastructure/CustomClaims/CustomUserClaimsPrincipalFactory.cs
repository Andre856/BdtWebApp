using Bdt.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace Bdt.Api.Infrastructure.CustomClaims;

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<UserEntity>
{
    public CustomUserClaimsPrincipalFactory(UserManager<UserEntity> userManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserEntity user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim("IsDarkTheme", user.IsDarkTheme.ToString()));

        return identity;
    }
}


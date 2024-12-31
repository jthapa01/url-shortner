using System.Security.Authentication;
using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue("preferred_username")
               ?? throw new AuthenticationException("Missing preferred_username claim");
    }
}
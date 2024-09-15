using System.Security.Claims;

namespace CaloryCalculation.Application.Helpers;

public static class ApplicationHelper
{
    public static string? GetUserIdByClaim(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
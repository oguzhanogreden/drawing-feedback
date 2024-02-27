using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication.Cognito;

public static class Extensions
{
    public static Guid? GetUserIdFromToken(this HttpContext httpContext)
    {
        var name = httpContext.User.Identity.Name;
        if (string.IsNullOrEmpty(name))
        {
            // Log or handle missing 'sub' claim as needed
            return null;
        }

        if (Guid.TryParse(name, out Guid userId))
        {
            return userId;
        }

        // Log or handle invalid format as needed
        return null;
    }
    
}
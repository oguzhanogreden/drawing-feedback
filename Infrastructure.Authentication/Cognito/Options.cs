using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.Cognito;

public static class Options
{
    public const string CognitoGroupsClaimType = "cognito:groups";
    
    public static JwtBearerOptions CreateCognitoOptions(this JwtBearerOptions options, string authority)
    {
        options.Authority = authority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",

            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authority,
        };

        return options;
    }
}
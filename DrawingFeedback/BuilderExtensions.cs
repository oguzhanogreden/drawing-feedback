using System.Security.Claims;
using DrawingFeedback.WebApplicationExtensions;
using Infrastructure.Authentication.Cognito;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DrawingFeedback;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddAdapters(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnonymousRequestAdapter, AnonymousRequestAdapter>();
        return builder;
    }

    public static WebApplicationBuilder AddJwtBearer(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication()
            .AddJwtBearer("Bearer", options =>
            {
                var userPoolId = builder.Configuration["Cognito:UserPoolId"];

                var authority = $"https://cognito-idp.eu-west-1.amazonaws.com/{userPoolId}";
                options.CreateCognitoOptions(authority);

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        var cognitoGroups = context.Principal.Claims
                            .Where(c => c.Type == Options.CognitoGroupsClaimType)
                            .Select(c => c.Value);

                        if (cognitoGroups.Contains("sudo"))
                        {
                            claimsIdentity.AddClaim(new Claim("scope", "feedback:request:add"));
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        return builder;
    }
}
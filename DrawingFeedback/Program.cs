using System.Security.Claims;
using Infrastructure.Authentication.Cognito;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

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
                    .Where(c => c.Type == Cognito.CognitoGroupsClaimType)
                    .Select(c => c.Value);

                // Check if the user is part of a specific group that should have the 'discounts:add' scope
                if (cognitoGroups.Contains("sudo"))
                {
                    // Add a custom 'scope' claim if the user is in the required group
                    claimsIdentity.AddClaim(new Claim("scope", "discounts:add"));
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
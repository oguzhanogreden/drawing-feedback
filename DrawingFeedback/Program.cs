using System.Security.Claims;
using DrawingFeedback;
using DrawingFeedback.WebApplicationExtensions;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Cognito;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.AddJwtBearer();

builder.AddAdapters();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.CreateApi();

app.Run();
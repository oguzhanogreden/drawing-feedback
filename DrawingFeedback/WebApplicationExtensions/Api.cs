using Infrastructure.Authentication.Cognito;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DrawingFeedback.WebApplicationExtensions;

public static class Api
{
    public static WebApplication? CreateApi(this WebApplication? webApplication)
    {
        webApplication?
            .MapGroup("/api/feedbacks")
            .MapFeedbacksApi();

        return webApplication;
    }

    private static RouteGroupBuilder MapFeedbacksApi(this RouteGroupBuilder group)
    {
        group.MapPost("request", RequestFeedback);

        return group;
    }

    private static async Task<IResult> RequestFeedback(HttpContext context,
        IAnonymousRequestAdapter anonymousRequestAdapter)
    {
        var accepted = false;

        var form = await context.Request.ReadFormAsync();

        var file = form.Files["file"];
        if (file is null)
        {
            return TypedResults.BadRequest("No file was provided");
        }

        var userId = context.GetUserIdFromToken();

        try
        {
            await anonymousRequestAdapter.RequestFeedback(userId, file);
        }
        catch 
        {
            // log exception
            return TypedResults.StatusCode(500);
        }

        return TypedResults.Ok(new RequestFeedbackResultDto(accepted));
    }
}

internal record RequestFeedbackResultDto(bool Accepted);
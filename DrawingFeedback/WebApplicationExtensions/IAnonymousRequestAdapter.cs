namespace DrawingFeedback.WebApplicationExtensions;

internal interface IAnonymousRequestAdapter
{
    Task RequestFeedback(Guid? userId, IFormFile file);
}
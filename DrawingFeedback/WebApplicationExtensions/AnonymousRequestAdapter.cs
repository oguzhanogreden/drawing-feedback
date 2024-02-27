namespace DrawingFeedback.WebApplicationExtensions;

class AnonymousRequestAdapter : IAnonymousRequestAdapter
{
    public Task RequestFeedback(Guid? userId, IFormFile file)
    {
        // TODO: upload the file
        // TODO: send for processing
        
        // throw new NotImplementedException();
        return Task.CompletedTask;
    }
}
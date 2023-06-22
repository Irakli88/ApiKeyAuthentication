namespace ApiKeyAuthentication.Authentication;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
        {
            return new UnauthorizedHttpObjectResult("API Key missing");
        }

        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        if (apiKey is not null && !apiKey.Equals(extractedApiKey))
        {
            return new UnauthorizedHttpObjectResult("Invalid API Key");
        }
        
        return await next(context);
    }
}
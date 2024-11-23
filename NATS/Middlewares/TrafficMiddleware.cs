namespace NATS.Middlewares;

public class TrafficMiddleware
{
    private readonly RequestDelegate _next;
    
    public TrafficMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, ITrafficService service)
    {
        string[] shouldIgnoreSegments = { "/quan-tri", "/js", "/css", "/images", "/api", "/ping" };
        bool shouldIgnore = shouldIgnoreSegments.Any(segment => context.Request.Path.StartsWithSegments(segment));

        if (!shouldIgnore)
        {
            string clientAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            if (clientAddress == "127.0.0.1")
            {
                clientAddress = "localhost";
            }
            string clientUserAgent = context.Request.Headers["User-Agent"].ToString();
            await service.RecordAsync(clientAddress, clientUserAgent);
        }
        await _next(context); 
    }
}
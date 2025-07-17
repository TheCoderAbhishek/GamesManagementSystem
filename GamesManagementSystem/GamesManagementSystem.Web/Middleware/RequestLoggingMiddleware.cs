namespace GamesManagementSystem.Web.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request details before it's processed further
            _logger.LogInformation("Incoming Request: {Method} {Path}", context.Request.Method, context.Request.Path);

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}

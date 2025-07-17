using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GamesManagementSystem.Web.Filters
{
    public class CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, IHostEnvironment hostEnvironment) : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger = logger;
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            // Log the detailed exception
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            // Create a user-friendly message, but show more details if in Development mode
            var errorMessage = _hostEnvironment.IsDevelopment()
                ? $"An unexpected error occurred: {context.Exception.Message}\n{context.Exception.StackTrace}"
                : "An unexpected error occurred. Please try again later.";

            var result = new ViewResult { ViewName = "Error" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(modelMetadata, context.ModelState)
            {
                { "ErrorMessage", errorMessage }
            };

            // Set the result to our custom error view
            context.Result = result;

            // Mark the exception as handled
            context.ExceptionHandled = true;
        }
    }
}

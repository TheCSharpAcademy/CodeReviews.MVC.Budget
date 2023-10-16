using System.Net;
using System.Text.Json;

namespace MVCBudget.Forser.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate requestDelegate { get; set; }
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate _requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            requestDelegate = _requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.ToString());
            //var errorMessageObject = new { Message = ex.Message, Code = "system_error" };

            //var errorMessage = JsonSerializer.Serialize(errorMessageObject);
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //return context.Response.WriteAsync(errorMessage);
        }
    }
}
namespace mp4MediaApp.Middleware
{
    using System.Net;
    using System.Text.Json;

    /// <summary>
    /// Defines the <see cref="GlobalExceptionMiddleware" />
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        /// <summary>
        /// Defines the _next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/></param>
        /// <param name="logger">The logger<see cref="ILogger{GlobalExceptionMiddleware}"/></param>
        public GlobalExceptionMiddleware(RequestDelegate next,
                                         ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// The InvokeAsync
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context,
                    "An unexpected server error occurred.",
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// The HandleExceptionAsync
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="statusCode">The statusCode<see cref="HttpStatusCode"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private static async Task HandleExceptionAsync(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            var isApiRequest = context.Request.Path.StartsWithSegments("/api");

            context.Response.StatusCode = (int)statusCode;

            if (isApiRequest)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}

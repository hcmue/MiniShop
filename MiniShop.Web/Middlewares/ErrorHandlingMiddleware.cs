namespace MiniShop.Web.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Gọi middleware tiếp theo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/html";

                var errorView = _env.IsDevelopment()
                    ? "<h1>Development Error</h1><pre>" + ex + "</pre>"
                    : "<h1>Đã có lỗi xảy ra</h1><p>Vui lòng thử lại sau.</p>";

                await context.Response.WriteAsync(errorView);
            }
        }
    }
}

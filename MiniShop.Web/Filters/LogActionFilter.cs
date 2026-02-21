using Microsoft.AspNetCore.Mvc.Filters;

namespace MiniShop.Web.Filters;

public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("🔄 Bắt đầu {Controller}/{Action}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"]);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("✅ Hoàn thành {Controller}/{Action}",
            context.RouteData.Values["controller"],
            context.RouteData.Values["action"]);
    }
}

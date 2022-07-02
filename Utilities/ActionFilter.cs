using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiKalum.Utilities
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> Logger;
        public ActionFilter(ILogger<ActionFilter> _logger)
        {
            this.Logger = _logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation("Esto se ejecuta antes de la accion a realizar");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation("Esto se ejecuta despues de la accion realizada");
        }
    }
}
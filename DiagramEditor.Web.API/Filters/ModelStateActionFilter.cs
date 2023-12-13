using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiagramEditor.Web.API.Filters;

public class ModelStateActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new ContentResult() { StatusCode = 400 };
        }

        base.OnActionExecuting(context);
    }
}

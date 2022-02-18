using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MVCPipeline.Filters
{
    public class EmptyActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("IActionFilter.OnActionExecuting");
            //context.Result = new ContentResult() { Content = "Short-circuit by ActionFilter " };
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"IActionFilter.OnActionExecuted: cancelled {context.Canceled}");
            //context.ExceptionHandled = true;
            //context.Result = new ContentResult() { Content = "Ignoring the action result" };
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MVCPipeline.Filters
{
    public class EmptyResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("IResourceFilter.OnResourceExecuting");
            //context.Result = new ContentResult()
            //{
            //    Content = "Short-circuit by ResourceFilter",
            //};
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine($"IResourceFilter.OnResourceExecuted: cancelled {context.Canceled}");
        }
    }
}

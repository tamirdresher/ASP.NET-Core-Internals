using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MVCPipeline.Filters
{
    public class EmptyExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine("IExceptionFilter.OnException");
            //context.ExceptionHandled = true;
            //context.Result = new ContentResult()
            //{
            //    Content = "Exception handled"
            //};
        }
    }
}

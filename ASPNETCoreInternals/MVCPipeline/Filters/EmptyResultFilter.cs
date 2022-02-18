using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MVCPipeline.Filters
{
    public class EmptyResultFilter : Attribute, IAlwaysRunResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("IAlwaysRunResultFilter.OnResultExecuting");
            //context.Cancel = true;
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("IAlwaysRunResultFilter.OnResultExecuted");
        }
    }
}

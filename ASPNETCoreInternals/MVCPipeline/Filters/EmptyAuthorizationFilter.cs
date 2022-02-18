using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MVCPipeline.Filters
{
    public class EmptyAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine($"IAuthorizationFilter.OnAuthorization({nameof(AuthorizationFilterContext)})");

            // Setting Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext.Result
            // to a non-null value inside an authorization filter will short-circuit the pipeline
            
            //context.Result = new ContentResult()
            //{
            //    Content = "Short-circuit by AuthorizationFilter ",
            //};
        }
    }
}

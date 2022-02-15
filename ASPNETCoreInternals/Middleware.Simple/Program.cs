using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Simple
{
    public class Program
    {
        public static void Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.Configure(app =>
                        app.Use(next =>
                            async (HttpContext context) =>
                            {
                                if (context.Request.Path == "/")
                                {
                                    await context.Response.WriteAsync("Hello World!");
                                    return;
                                }

                                await next(context);
                            }

                        )))
                        .Build()
                        .Run();
    }
}

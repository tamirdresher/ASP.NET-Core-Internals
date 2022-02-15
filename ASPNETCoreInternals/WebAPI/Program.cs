using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main2(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.Configure(app =>
                        app.Use(next =>
                            async (context) =>
                                {
                                    if (context.Request.Path == "/")
                                    {
                                        await context.Response.WriteAsync("Hello World!");
                                    }

                                    await next(context);
                                }

                        )))
                        .Build()
                        .Run();

        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

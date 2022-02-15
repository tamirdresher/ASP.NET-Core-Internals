using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyCustomerMiddlewares();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Middleware", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Middleware v1"));
            }

            // Comment this to make sure the next middlewares run
            //app.Run(async (context) =>
            //{
            //    if (context.Request.Path == "/")
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //        return;
            //    }
            //});


            app.Use((next) => async (httpCtx) => { await next(httpCtx); });
            app.Use(async (httpCtx, next) => await next());
            app.UseMyInlineCustomProfilerMiddleware();
            app.UseMyConventionBasedCustomProfilerMiddleware();
            app.UseMyFactoryBasedCustomProfilerMiddleware();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyInlineCustomProfilerMiddleware(this IApplicationBuilder app)
        {
            return app.Use(async (httpCtx, next) =>
            {
                var sw=Stopwatch.StartNew();
                try
                {
                    await next();
                }
                finally
                {
                    Console.WriteLine($"MyProfiler: {httpCtx.Request} exexuted {sw.ElapsedMilliseconds} miliseconds");
                }
            });
        }
        public static IServiceCollection AddMyCustomerMiddlewares(this IServiceCollection services)
        {
            return services.AddTransient<MyFactoryBasedProfilerMiddleware>();
        }

        public static IApplicationBuilder UseMyFactoryBasedCustomProfilerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyFactoryBasedProfilerMiddleware>();
        }
        class MyFactoryBasedProfilerMiddleware : IMiddleware
        {
            public MyFactoryBasedProfilerMiddleware() {}
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    await next(context);
                }
                finally
                {
                    Console.WriteLine($"MyFactoryBasedProfiler: {context.Request} executed {sw.ElapsedMilliseconds} miliseconds");
                }
            }
        }

        public static IApplicationBuilder UseMyConventionBasedCustomProfilerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyConventionBasedProfilerMiddleware>();
        }
        class MyConventionBasedProfilerMiddleware
        {
            private readonly RequestDelegate _next;

            public MyConventionBasedProfilerMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    await _next(context);
                }
                finally
                {
                    Console.WriteLine($"ConventionBasedProfiler: {context.Request} executed {sw.ElapsedMilliseconds} miliseconds");
                }
            }
        }
    }
}

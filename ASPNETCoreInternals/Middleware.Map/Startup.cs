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
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Map
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Middleware.Map", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.Map("/dev", (appBuilder) =>
            //{
            //    if (env.IsDevelopment())
            //    {
            //        appBuilder.UseDeveloperExceptionPage();
            //        appBuilder.UseSwagger();
            //        appBuilder.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Middleware.Map v1"));
            //    }
            //    UseDefaultPipeline(appBuilder);

            //    appBuilder.Run(async (context) => {
            //        await context.Response.WriteAsync("prod");
            //    });
            //});

            //app.MapWhen((HttpContext context) => context.Request.Query.ContainsKey("prod"), (appBuilder) =>
            //{
            //    UseDefaultPipeline(appBuilder);
            //    appBuilder.Use(async (context, next) =>
            //    {
            //        await context.Response.WriteAsync("dev");
            //        await next();
            //    });

            //});

            app.Map("/s", cfg =>
            {

            });
            app.Map("/dev", appBuilder =>
            {
                appBuilder.UsePathBase("/dev");
                if (env.IsDevelopment())
                {
                    appBuilder.UseDeveloperExceptionPage();
                    appBuilder.UseSwagger();
                    appBuilder.UseSwaggerUI(c => c.SwaggerEndpoint("/dev/swagger/v1/swagger.json", "Middleware.Map v1"));
                }
                appBuilder.Use(async (ctx, next) => { await next(); });
                UseDefaultPipeline(appBuilder);
            });

            app.Map("/prod", appBuilder =>
            {
                appBuilder.UsePathBase("/prod");
                appBuilder.Use(async (ctx, next) => { await next(); });
                UseDefaultPipeline(appBuilder);
            });



            static void UseDefaultPipeline(IApplicationBuilder app)
            {
                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}

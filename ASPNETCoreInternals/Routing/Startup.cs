using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routing
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
            services.AddSignalR();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Routing", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Routing v1"));
            }

            app.UseHttpsRedirection();

            // Location 1: before routing runs, endpoint is always null here
            app.Use(next => context =>
            {
                var endpointFeature = context.Features.Get<IEndpointFeature>();
                Console.WriteLine($"1. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return next(context);
            });

            app.UseRouting();

            // Location 2: after routing runs, endpoint will be non-null if routing found a match
            app.Use(next => context =>
            {
                var endpointBuilder = app.Properties["__EndpointRouteBuilder"];
                var endpointFeature = context.Features.Get<IEndpointFeature>();
                Console.WriteLine($"2. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                // Location 3: runs when this endpoint matches
                endpoints.MapGet("/", context =>
                {
                    Console.WriteLine(
                        $"3. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                    return Task.CompletedTask;
                }).WithDisplayName("Hello");

                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });

            // Location 4: runs after UseEndpoints - will only run if there was no match
            app.Use(next => context =>
            {
                Console.WriteLine($"4. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
                return next(context);
            });

        }
    }

    public class ChatHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

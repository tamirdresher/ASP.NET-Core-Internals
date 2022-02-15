using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerAndHttpApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            KestrelServerOptions kestrelOptions = new KestrelServerOptions()
            {
                ApplicationServices = new ServiceCollection().AddLogging().BuildServiceProvider()
            };
            var socketOptions = new SocketTransportOptions();
            var server = new KestrelServer(Options.Create(kestrelOptions), 
                                           new SocketTransportFactory( Options.Create(socketOptions), NullLoggerFactory.Instance),
                                           NullLoggerFactory.Instance);

            await server.StartAsync(new HttpApplication(), CancellationToken.None);

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();
            foreach (var address in serverAddressesFeature.Addresses)
            {
                Console.WriteLine($"Server listening on {address}");
            }
            Console.ReadLine();
        }

        class HttpApplication : IHttpApplication<HttpContext>
        {
            public HttpContext CreateContext(IFeatureCollection contextFeatures)
            {
                return new DefaultHttpContext(contextFeatures);
            }

            public void DisposeContext(HttpContext context, Exception exception)
            {
                
            }

            public async Task ProcessRequestAsync(HttpContext context)
            {
                await context.Response.WriteAsync(DateTime.Now.ToString());
            }
        }

    }
}

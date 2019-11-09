using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicationServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace CommunicationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Data.PopulateList();
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    //Windows
                    webBuilder.UseStartup<Startup>();
                    
                    
                    //uncomment for mac
                    //webBuilder.UseStartup<Startup>().ConfigureKestrel(options =>
                    //{
                    //    options.Limits.MinRequestBodyDataRate = null;

                    //    options.ListenAnyIP(5000,
                    //          listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });

                    //    //options.ListenAnyIP(5001,
                    //    //   listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                    //});

                });
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
                // BuildWebHost(args).Run();
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) =>
                     {
                         var env = hostingContext.HostingEnvironment;
                         config.SetBasePath(Directory.GetCurrentDirectory());
                         config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false);
                         config.AddEnvironmentVariables();
                     })
                     .ConfigureLogging((hostingContext, logging) =>
                     {
                         logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                         logging.AddConsole();
                         logging.AddDebug();
                     })
                    .UseStartup<Startup>()
                    .UseUrls("http://*:5000")
                    .Build();

                    host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}

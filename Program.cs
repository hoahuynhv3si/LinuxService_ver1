using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Logging; 
using Serilog;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace myservice
{
    class Program
    {
        public static Dictionary<string,string> RedisLog {get; private set;}
        static async Task Main(string[] args)
        {
            IHost host = new HostBuilder() 
            
            .ConfigureHostConfiguration(configHost =>  
                 {  
                     configHost.SetBasePath(Directory.GetCurrentDirectory());  
                     configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");  
                     //configHost.AddCommandLine(args); 
                    // Console.WriteLine(args);
                 })  
                 
            .ConfigureAppConfiguration((hostContext, configApp) =>  
                 {  
                     configApp.SetBasePath(Directory.GetCurrentDirectory());  
                     configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");  
                     configApp.AddJsonFile($"appsettings.json", true);  
                     configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);  
                     //configApp.AddCommandLine(args);  
                 })   
                 
                 .ConfigureServices((hostContext, services) =>  
                {  
                    services.AddLogging();
                    //services.AddHostedService<ApplicationLifetimeHostedService>();   
                    services.AddSingleton<IHostedService, RedisService>();
                })  
                
                .ConfigureLogging((hostContext, configLogging) =>  
                {  
                    configLogging.AddSerilog(new LoggerConfiguration()  
                              .ReadFrom.Configuration(hostContext.Configuration)  
                              .CreateLogger());  
                })  
                
            .Build();  


            /* 
            var redis = RedisConnectorHelper.Connection.GetDatabase();
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("chanel-1", (channel, message) =>
            {
                Console.WriteLine(message);
                using (StreamWriter writter = System.IO.File.AppendText("logFile.txt"))
                {
                    writter.WriteLine($"[{DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}] {(string)message}");
                }
            });

            */

            await host.RunAsync();  
        }
    }
}

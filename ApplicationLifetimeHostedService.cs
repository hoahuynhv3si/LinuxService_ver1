using Microsoft.Extensions.Configuration;  
using Microsoft.Extensions.Hosting;  
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading;  
using System.Threading.Tasks;  
  
namespace myservice  
{  
    public class ApplicationLifetimeHostedService : IHostedService  
    {  
        IApplicationLifetime appLifetime;  
        ILogger<ApplicationLifetimeHostedService> logger;  
        IHostingEnvironment environment;  
        IConfiguration configuration;  
        private ConnectionMultiplexer connection;
        public ApplicationLifetimeHostedService(  
            IConfiguration configuration,  
            IHostingEnvironment environment,  
            ILogger<ApplicationLifetimeHostedService> logger,   
            IApplicationLifetime appLifetime)  
        {  
            this.configuration = configuration;  
            this.logger = logger;  
            this.appLifetime = appLifetime;  
            this.environment = environment;  
            
            var options = new ConfigurationOptions();
            options.EndPoints.Add(this.configuration.GetSection("Redis:Host").Value);
            options.EndPoints.Add(this.configuration.GetSection("Redis:Port").Value);
            connection = ConnectionMultiplexer.Connect(options);
        
        }  
  
        public Task StartAsync(CancellationToken cancellationToken)  
        {  
            this.logger.LogInformation("StartAsync method called.");  
            Console.WriteLine("StartAsync method called.");  
  
            this.appLifetime.ApplicationStarted.Register(OnStarted);  
            this.appLifetime.ApplicationStopping.Register(OnStopping);  
            this.appLifetime.ApplicationStopped.Register(OnStopped);  
  
            return Task.CompletedTask;  
  
        }  
  
        private void OnStarted()  
        {  
            this.logger.LogInformation("OnStarted method called.");  
            Console.WriteLine("OnStarted method called.");  

            // Post-startup code goes here 

            var sub = connection.GetSubscriber();
            sub.Subscribe("chanel-2", (channel, message) =>
            {
                Console.WriteLine(message);
                this.logger.LogDebug($"StartAsync method : {message}");  
            });
 
        }  
  
        private void OnStopping()  
        {  
            this.logger.LogInformation("OnStopping method called.");  
            this.logger.LogInformation("OnStopping method called.");  
  
            // On-stopping code goes here  
        }  
  
        private void OnStopped()  
        {  
            this.logger.LogInformation("OnStopped method called.");  
            this.logger.LogInformation("OnStopped method called.");  
  
            // Post-stopped code goes here  
        }  
  
  
        public Task StopAsync(CancellationToken cancellationToken)  
        {  
            this.logger.LogInformation("StopAsync method called.");  
            this.logger.LogInformation("StopAsync method called.");  
  
            return Task.CompletedTask;  
        }  
    }  
}  
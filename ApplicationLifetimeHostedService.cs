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
        private readonly IApplicationLifetime appLifetime;  
        private readonly ILogger<ApplicationLifetimeHostedService> logger;  
        private readonly IHostingEnvironment environment;  
        private readonly IConfiguration configuration;  
        private readonly IRedisConnectorHelper redisConnectorHelper;
        
        public ApplicationLifetimeHostedService(  
            IConfiguration configuration,  
            IHostingEnvironment environment,  
            ILogger<ApplicationLifetimeHostedService> logger,   
            IApplicationLifetime appLifetime,
            IRedisConnectorHelper redisConnectorHelper)  
        {  
            this.configuration = configuration;  
            this.logger = logger;  
            this.appLifetime = appLifetime;  
            this.environment = environment;  
            this.redisConnectorHelper = redisConnectorHelper;
        
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

            var sub = this.redisConnectorHelper.Connection.GetSubscriber();
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
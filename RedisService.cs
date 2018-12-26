using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace myservice
{
    public class RedisService : IHostedService
    {
        private readonly IApplicationLifetime appLifetime;  
        private readonly ILogger<ApplicationLifetimeHostedService> logger;  
        private readonly IHostingEnvironment environment;  
        private readonly IConfiguration configuration;  
        private readonly IRedisConnectorHelper redisConnectorHelper;

        public RedisService(
            IConfiguration configuration,  
            IHostingEnvironment environment,  
            ILogger<ApplicationLifetimeHostedService> logger,   
            IApplicationLifetime appLifetime,
            IRedisConnectorHelper redisConnectorHelper
        )
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

  
        public Task StopAsync(CancellationToken cancellationToken)  
        {  
            this.logger.LogInformation("StopAsync method called.");  
  
            return Task.CompletedTask;  
        }  
    }
}
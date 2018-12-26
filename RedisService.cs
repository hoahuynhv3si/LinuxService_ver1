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
        private readonly ILogger<RedisService> logger;  
        private readonly IHostingEnvironment environment;  
        private readonly IConfiguration configuration;  
        private readonly IRedisConnectorHelper redisConnectorHelper;

        public RedisService(
            IConfiguration configuration,  
            IHostingEnvironment environment,  
            ILogger<RedisService> logger,   
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
            this.appLifetime.ApplicationStarted.Register(Subscribe);   
            return Task.CompletedTask;  
        }  
  
        public Task StopAsync(CancellationToken cancellationToken)  
        {  
            this.logger.LogInformation("StopAsync method called.");  
            return Task.CompletedTask;  
        }  

        private void Subscribe()
        {
            this.logger.LogInformation("Subscribe method called.");  
            Console.WriteLine("Subscribe chanel-2");  

            var sub = this.redisConnectorHelper.Connection.GetSubscriber();
            sub.Subscribe("chanel-2", (channel, message) =>
            {
                Console.WriteLine(message);
                this.logger.LogDebug($"Subscribe method : {message}");  
            });
        }
    }
}
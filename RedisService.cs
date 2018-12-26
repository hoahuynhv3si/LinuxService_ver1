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
        private ConnectionMultiplexer connection;
        private readonly IConfiguration configuration;  
        private readonly ILogger<ApplicationLifetimeHostedService> logger;  

        public RedisService(
            IConfiguration configuration,
            ILogger<ApplicationLifetimeHostedService> logger
        )
        {
            this.configuration = configuration; 
            this.logger = logger;  

            var options = new ConfigurationOptions();
            options.EndPoints.Add(this.configuration.GetSection("Redis:Host").Value);
            options.EndPoints.Add(this.configuration.GetSection("Redis:Port").Value);
            connection = ConnectionMultiplexer.Connect(options);
        
        }

        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StartAsync Method Subscribe chanel-2");
            var sub = connection.GetSubscriber();
            sub.Subscribe("chanel-2", (channel, message) =>
            {
                Console.WriteLine(message);
                this.logger.LogDebug($"StartAsync method : {message}");  
            });

            return Task.CompletedTask;
        } 
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogDebug("StopAsync method called.");  
  
            return Task.CompletedTask; 
        }
    }
}
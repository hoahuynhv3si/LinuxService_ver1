using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace myservice
{
    public interface IRedisConnectorHelper{
        ConnectionMultiplexer Connection {get;}
    }
    public class RedisConnectorHelper : IRedisConnectorHelper
    {
        private readonly Lazy<ConnectionMultiplexer> lazyConnection;
        private readonly IConfiguration configuration;  

        public RedisConnectorHelper(IConfiguration configuration)
        {
            this.configuration = configuration;  

            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var options = new ConfigurationOptions();
                options.EndPoints.Add(this.configuration.GetSection("Redis:Host").Value);
                options.EndPoints.Add(this.configuration.GetSection("Redis:Port").Value);
                return ConnectionMultiplexer.Connect(options);
            });
        }

        public ConnectionMultiplexer Connection => lazyConnection.Value;
    }
}

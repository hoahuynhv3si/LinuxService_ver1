using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace myservice
{
    public interface IRedisConnectorHelper{
    }
    public class RedisConnectorHelper
    {
        private readonly Lazy<ConnectionMultiplexer> lazyConnection;
        private readonly IConfiguration configuration;  

        public RedisConnectorHelper(IConfiguration configuration)
        {
            this.configuration = configuration;  

            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
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

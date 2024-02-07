using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Api.Services
{
    public class ServiceBus : IServiceBus
    {
        private readonly IConfiguration _config;

        public ServiceBus(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string topicName)
        {
            var topicClient = new TopicClient(_config.GetConnectionString("AzureServiceBus"), topicName);

            string messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await topicClient.SendAsync(message);
        }
    }
}

using FollowService.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowService.Tests.IntegrationTests
{
    public class MockServiceBus : IServiceBus
    {
        public Task SendMessageAsync<T>(T serviceBusMessage, string topicName)
        {
            Console.WriteLine("Message sent");
            return Task.CompletedTask;
        }
    }
}

using AccountService.Api.Data;
using AccountService.Api.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AccountService.Api.Services
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly IConfiguration _config;
        private readonly ISubscriptionClient subscriptionClient;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private const string topicName = "user-registration";
        private const string subscriptionName = "account-service";

        public ServiceBusConsumer(IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _config = config;
            subscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureServiceBus"), topicName, subscriptionName);
            _contextFactory = contextFactory;
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var account = JsonSerializer.Deserialize<Account>(jsonString);
                account.Name = account.Username;

                Console.WriteLine($"Account received: {account.Email}");

                using (var db = _contextFactory.CreateDbContext())
                {
                    await db.Accounts.AddAsync(account, token);
                    await db.SaveChangesAsync(token);
                }

                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}

using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TimelineService.Api.Data;
using TimelineService.Api.Models;
using TimelineService.Api.Models.Messages;

namespace TimelineService.Api.Services
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ServiceBusConsumer> _logger;
        private readonly ISubscriptionClient registrationSubscriptionClient;
        private readonly ISubscriptionClient postQuackSubscriptionClient;
        private readonly ISubscriptionClient deleteQuackSubscriptionClient;
        private readonly ISubscriptionClient followSubscriptionClient;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private const string userRegistrationTopic = "user-registration";
        private const string postQuackTopic = "post-quack";
        private const string deleteQuackTopic = "delete-quack";
        private const string followTopic = "follow";
        private const string subscriptionName = "timeline-service";

        public ServiceBusConsumer(IConfiguration config, IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<ServiceBusConsumer> logger)
        {
            _config = config;
            registrationSubscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureServiceBus"), userRegistrationTopic, subscriptionName);
            postQuackSubscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureServiceBus"), postQuackTopic, subscriptionName);
            deleteQuackSubscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureServiceBus"), deleteQuackTopic, subscriptionName);
            followSubscriptionClient = new SubscriptionClient(_config.GetConnectionString("AzureServiceBus"), followTopic, subscriptionName);
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            registrationSubscriptionClient.RegisterMessageHandler(ProcessRegistrationMessageAsync, messageHandlerOptions);
            postQuackSubscriptionClient.RegisterMessageHandler(ProcessPostQuackMessageAsync, messageHandlerOptions);
            deleteQuackSubscriptionClient.RegisterMessageHandler(ProcessDeleteQuackMessageAsync, messageHandlerOptions);
            followSubscriptionClient.RegisterMessageHandler(ProcessFollowMessageAsync, messageHandlerOptions);
        }

        private async Task ProcessRegistrationMessageAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var user = JsonSerializer.Deserialize<User>(jsonString);

                _logger.LogInformation($"Account received: {user.Email}");

                using (var db = _contextFactory.CreateDbContext())
                {
                    await db.Users.AddAsync(user, token);
                    await db.SaveChangesAsync(token);
                }

                _logger.LogInformation($"User stored in database with id: {user.UserId}");

                await registrationSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong during ProcessRegistrationMessageAsync");
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task ProcessPostQuackMessageAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var quack = JsonSerializer.Deserialize<Quack>(jsonString);

                _logger.LogInformation($"Quack received: {quack.Message}");

                using (var db = _contextFactory.CreateDbContext())
                {
                    await db.Quacks.AddAsync(quack, token);
                    await db.SaveChangesAsync(token);
                }

                _logger.LogInformation($"Quack stored in database with id: {quack.Id}");

                await postQuackSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong during ProcessPostQuackMessageAsync");
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task ProcessDeleteQuackMessageAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var quackId = JsonSerializer.Deserialize<Guid>(jsonString);

                _logger.LogInformation($"Quack received: {quackId}");

                using (var db = _contextFactory.CreateDbContext())
                {
                    var quack = await db.Quacks.FirstOrDefaultAsync(x=> x.Id == quackId, token);

                    if (quack == null)
                    {
                        throw new Exception("Quack not found");
                    }

                    db.Quacks.Remove(quack);
                    await db.SaveChangesAsync(token);
                }

                _logger.LogInformation($"Quack deleted in database with id: {quackId}");

                await deleteQuackSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong during ProcessDeleteQuackMessageAsync");
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task ProcessFollowMessageAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var followMessage = JsonSerializer.Deserialize<NewFollowingMessage>(jsonString);

                _logger.LogInformation($"Follow message received from user: {followMessage.UserId}");

                using (var db = _contextFactory.CreateDbContext())
                {
                    if (followMessage.Follow) // if following
                    {
                        var user = await db.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == followMessage.UserId, token);
                        var userToFollow = await db.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.UserId == followMessage.UserToFollowId, token);

                        user.Following.Add(userToFollow);
                        userToFollow.Followers.Add(user);

                        await db.SaveChangesAsync(token);

                        _logger.LogInformation($"user {user.UserId} Sucessfully followed user {userToFollow.UserId}");
                    }
                    else if (!followMessage.Follow) // if unfollowing
                    {
                        var user = await db.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == followMessage.UserId, token);
                        var userToUnfollow = await db.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.UserId == followMessage.UserToFollowId, token);

                        user.Following.Remove(userToUnfollow);
                        userToUnfollow.Followers.Remove(user);

                        await db.SaveChangesAsync(token);

                        _logger.LogInformation($"user {userToUnfollow.UserId} Sucessfully followed user {user.UserId}");
                    }
                }

                await followSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong during ProcessFollowMessageAsync");
                _logger.LogError(e.Message);
                throw;
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            _logger.LogError($"Message handler exception: {arg.Exception}");
            return Task.CompletedTask;
        }
    }
}

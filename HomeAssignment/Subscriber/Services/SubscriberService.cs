using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Subscriber.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber.Services
{
    public class SubscriberService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private SubscriptionName subscriptionName;

        public SubscriberService(IOptions<GCPSettings> settings)
        {
            subscriptionName = new SubscriptionName("swd63adphome", "my-sub");
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("SubscriberService running.");

            _timer = new Timer(async _ => await DoWork(_), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async Task DoWork(object? state)
        {
            SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
            List<PubsubMessage> receivedMessages = new List<PubsubMessage>();
            await subscriber.StartAsync((msg, cancellationToken) =>
            {
                receivedMessages.Add(msg);
                var messageData = JsonConvert.DeserializeObject<dynamic>(msg.Data.ToStringUtf8());
                Console.WriteLine($"Received message {msg.MessageId} from {messageData.Source} published at {msg.PublishTime.ToDateTime()}");
                Console.WriteLine("");
                Console.WriteLine($"Text: '{messageData.Message}'");
                Console.WriteLine("");
                subscriber.StopAsync(TimeSpan.FromSeconds(5));
                return Task.FromResult(SubscriberClient.Reply.Ack);
            });
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("SubscriberService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

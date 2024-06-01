using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Options;
using Publisher.Models;
using Newtonsoft.Json;

namespace Publisher.Services
{
    public class PublisherService
    {
        private readonly TopicName topicName;
        public PublisherService(IOptions<GCPSettings> settings)
        {
            topicName = new TopicName("swd63adphome", "my-topic");
        }

        public async Task PublishMessage(string message, string source)
        {
            PublisherClient publisher = PublisherClient.Create(topicName);
            var messageWithSource = new
            {
                Source = source,
                Message = message
            };

            string messageId = await publisher.PublishAsync(JsonConvert.SerializeObject(messageWithSource));
            Console.WriteLine(messageId);
            await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));
        }
    }
}

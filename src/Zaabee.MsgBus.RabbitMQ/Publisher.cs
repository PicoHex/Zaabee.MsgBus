using System.Threading;
using System.Threading.Tasks;
using Zaabee.MsgBus.Abstractions;
using Zaabee.RabbitMQ.Abstractions;

namespace Zaabee.MsgBus.RabbitMQ
{
    public class Publisher : IZaabeePublisher
    {
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;

        public Publisher(IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }

        public void Publish<T>(T message)
        {
            _rabbitMqClient.PublishEvent(message);
        }

        public void Publish<T>(string topic, T message)
        {
            _rabbitMqClient.PublishEvent(topic, message);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            await _rabbitMqClient.PublishEventAsync(message);
        }

        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        {
            await _rabbitMqClient.PublishEventAsync(topic, message);
        }
    }
}
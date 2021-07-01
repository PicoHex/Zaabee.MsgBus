using System;
using System.Threading;
using System.Threading.Tasks;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus.RabbitMQ
{
    public class Publisher : IZaabeePublisher
    {
        public void Publish<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Publish<T>(string topic, T message)
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
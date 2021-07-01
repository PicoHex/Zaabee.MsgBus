using System;
using System.Data;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Zaabee.MsgBus.Abstractions;
using Zaabee.MsgBus.DapperExtensions;
using Zaabee.SequentialGuid;

namespace Zaabee.MsgBus
{
    public class ZaabeeMsgBus : IZaabeeMsgBus
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public ZaabeeMsgBus(IDbTransaction transaction)
        {
            _connection = transaction.Connection;
            _transaction = transaction;
        }

        public void Publish<T>(T message) => Publish(typeof(T).Namespace, message);

        public void Publish<T>(string topic, T message)
        {
            var unpublishedMessage = new UnpublishedMessage
            {
                Id = SequentialGuidHelper.GenerateComb(),
                Topic = topic,
                Data = JsonSerializer.Serialize(message),
                PersistentUtcTime = DateTime.UtcNow
            };
            _connection.Add(unpublishedMessage, _transaction);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) =>
            await PublishAsync(typeof(T).Namespace, message, cancellationToken);

        public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
        {
            var unpublishedMessage = new UnpublishedMessage
            {
                Id = SequentialGuidHelper.GenerateComb(),
                Topic = topic,
                Data = JsonSerializer.Serialize(message),
                PersistentUtcTime = DateTime.UtcNow
            };
            await _connection.AddAsync(unpublishedMessage, _transaction);
        }
    }
}
using System;
using System.Data;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Zaabee.Dapper.Extensions;
using Zaabee.MsgBus.Abstractions;
using Zaabee.SequentialGuid;

namespace Zaabee.MsgBus
{
    public class ZaabeeMsgBus : IZaabeeMsgBus
    {
        private IDbConnection Connection { get; set; }
        private IDbTransaction _transaction;

        public IDbTransaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                Connection = _transaction.Connection;
            }
        }

        public ZaabeeMsgBus()
        {

        }

        public ZaabeeMsgBus(IDbTransaction transaction)
        {
            Connection = transaction.Connection;
            Transaction = transaction;
        }

        public void Publish<T>(T message) =>
            Publish(typeof(T).Namespace, message);

        public void Publish<T>(string topic, T message)
        {
            var unpublishedMessage = new UnpublishedMessage
            {
                Id = SequentialGuidHelper.GenerateComb(),
                Topic = topic,
                Data = JsonSerializer.Serialize(message),
                CreatedUtcTime = DateTime.UtcNow
            };
            Connection.Add(unpublishedMessage, Transaction);
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
                CreatedUtcTime = DateTime.UtcNow
            };
            await Connection.AddAsync(unpublishedMessage, Transaction);
        }
    }
}
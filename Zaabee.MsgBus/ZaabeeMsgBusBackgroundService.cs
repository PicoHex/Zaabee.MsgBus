using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaabee.MsgBus.Abstractions;
using Zaabee.MsgBus.DapperExtensions;
using Zaabee.SequentialGuid;

namespace Zaabee.MsgBus
{
    public class ZaabeeMsgBusBackgroundService : BackgroundService
    {
        private readonly IZaabeePublisher _publisher;
        private readonly IServiceProvider _serviceProvider;

        public ZaabeeMsgBusBackgroundService(IZaabeePublisher publisher, IServiceProvider serviceProvider)
        {
            _publisher = publisher;
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastPublishTime = DateTime.UtcNow;
            while (!stoppingToken.IsCancellationRequested)
            {
                using var connection = _serviceProvider.GetService<IDbConnection>();
                if (connection is null) throw new NullReferenceException(nameof(connection));
                var ms = DateTime.UtcNow - lastPublishTime;
                if (ms.Milliseconds < 100)
                    await Task.Delay(100 - ms.Milliseconds, stoppingToken);

                var unpublishedMessages = await connection.TakeAsync<UnpublishedMessage>(10);

                foreach (var unpublishedMessage in unpublishedMessages)
                    await _publisher.PublishAsync(unpublishedMessage.ToCloudEvent(), stoppingToken);

                await connection.DeleteAsync<UnpublishedMessage>(unpublishedMessages.Select(p => p.Id));
                await connection.AddRangeAsync(unpublishedMessages.Select(p => new PublishedMessage(p)).ToList());

                lastPublishTime = DateTime.UtcNow;
            }
        }
    }
}
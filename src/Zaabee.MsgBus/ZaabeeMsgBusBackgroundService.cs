using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaabee.Dapper.Extensions;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus
{
    public class ZaabeeMsgBusBackgroundService : BackgroundService
    {
        private readonly IZaabeePublisher _publisher;
        private readonly IServiceProvider _serviceProvider;

        public ZaabeeMsgBusBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _publisher = serviceProvider.GetService<IZaabeePublisher>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastPublishTime = DateTime.UtcNow;
            while (!stoppingToken.IsCancellationRequested)
            {
                using var connection = _serviceProvider.GetService<IDbConnection>();
                if (connection is null)
                    throw new NullReferenceException(nameof(connection));
                var ms = DateTime.UtcNow - lastPublishTime;
                if (ms.Milliseconds < 100)
                    await Task.Delay(100 - ms.Milliseconds, stoppingToken);

                var unpublishedMessages = await connection.TakeAsync<UnpublishedMessage>(10);

                foreach (var unpublishedMessage in unpublishedMessages)
                    await _publisher.PublishAsync(unpublishedMessage.ToCloudEvent(), stoppingToken);

                await connection.DeleteByIdsAsync<UnpublishedMessage>(
                    unpublishedMessages.Select(p => p.Id)
                );
                await connection.AddRangeAsync(
                    unpublishedMessages.Select(p => p.ToPublishedMessage()).ToList()
                );

                lastPublishTime = DateTime.UtcNow;
            }
        }
    }
}

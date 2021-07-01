using System;
using CloudNative.CloudEvents;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus
{
    public static class UnpublishedMessageExtensions
    {
        public static CloudEvent ToCloudEvent(this UnpublishedMessage message)
        {
            var cloudEvent = new CloudEvent(new []
            {
                CloudEventAttribute.CreateExtension("retryLimit",CloudEventAttributeType.Integer),
                CloudEventAttribute.CreateExtension("retry",CloudEventAttributeType.Integer)
            })
            {
                Id = message.Id.ToString(),
                Type = message.Topic,
                Source = new Uri(""),
                Data = message.Data,
                DataContentType = "application/json",
                DataSchema = new Uri(""),
                Subject = "",
                Time = DateTimeOffset.UtcNow
            };
            return cloudEvent;
        }
    }
}
using System;
using CloudNative.CloudEvents;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus
{
    public static class UnpublishedMessageExtensions
    {
        internal static CloudEvent ToCloudEvent(this UnpublishedMessage message) =>
            new(new[]
            {
                CloudEventAttribute.CreateExtension("retryLimit", CloudEventAttributeType.Integer),
                CloudEventAttribute.CreateExtension("retry", CloudEventAttributeType.Integer)
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

        internal static PublishedMessage ToPublishedMessage(this UnpublishedMessage message) =>
            new()
            {
                Id = message.Id,
                Topic = message.Topic,
                Data = message.Data,
                PersistentUtcTime = message.CreatedUtcTime,
                PublishedUtcTime = DateTime.UtcNow
            };
    }
}
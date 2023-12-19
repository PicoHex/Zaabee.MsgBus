using System;

namespace Zaabee.MsgBus.Abstractions
{
    public class PublishedMessage
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public string Data { get; set; }
        public DateTime PersistentUtcTime { get; set; }
        public DateTime PublishedUtcTime { get; set; }
    }
}

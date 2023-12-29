namespace Zaabee.MsgBus.Abstractions;

public class UnpublishedMessage
{
    public Guid Id { get; set; }
    public string Topic { get; set; }
    public string Data { get; set; }
    public DateTime CreatedUtcTime { get; set; }
}

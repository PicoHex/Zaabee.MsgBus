using System.Threading;
using System.Threading.Tasks;

namespace Zaabee.MsgBus.Abstractions
{
    public interface IZaabeeMsgBus
    {
        void Publish<T>(T message);
        void Publish<T>(string topic, T message);
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
        Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
    }
}
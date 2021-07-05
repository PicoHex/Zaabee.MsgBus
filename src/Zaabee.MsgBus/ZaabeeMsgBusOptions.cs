using System.Data;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus
{
    public class ZaabeeMsgBusOptions
    {
        public IDbTransaction Transaction { get; set; }
    }
}
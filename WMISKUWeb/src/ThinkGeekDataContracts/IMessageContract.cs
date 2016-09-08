using System.Runtime.Serialization;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface IMessageContract
    {
        string ID { get; set; }
        string TransmitTime { get; set; }
        IMessageTarget Target { get; set; }
        string MessageType { get; set; }
    }
}

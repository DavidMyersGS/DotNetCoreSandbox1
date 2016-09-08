using System.Runtime.Serialization;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public class MessageTarget
    {
        public string Company { get; set; }

        public string Warehouse { get; set; }

        public string Brand { get; set; }
    }
}

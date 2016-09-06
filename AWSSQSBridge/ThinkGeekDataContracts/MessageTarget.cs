using System.Runtime.Serialization;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    [DataContract]
    public class MessageTarget : IMessageTarget
    {
        [DataMember(Name = "company", IsRequired = true)]
        public string Company { get; set; }

        [DataMember(Name = "warehouse", IsRequired = true)]
        public string Warehouse { get; set; }

        [DataMember(Name = "brand", IsRequired = true)]
        public string Brand { get; set; }
    }
}

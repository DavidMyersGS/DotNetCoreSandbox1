using System.Runtime.Serialization;


namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    [KnownType(typeof(MessageTarget))]
    [KnownType(typeof(Identifiers))]
    [KnownType(typeof(Dimensions))]
    [DataContract]
    public class SKUUpsertMessageContract : ISKUUpsertMessageContract
    {
        [DataMember(Name = "ID", IsRequired = true)]
        public string ID { get; set; }

        [DataMember(Name = "transmit_time", IsRequired = true)]
        public string TransmitTime { get; set; }

        [DataMember(Name = "target", IsRequired = true)]
        public IMessageTarget Target { get; set; }

        [DataMember(Name = "message_type", IsRequired = true)]
        public string MessageType { get; set; }

        [DataMember(Name = "sku", IsRequired = true)]
        public string sku { get; set; }
        [DataMember(Name = "product_id", IsRequired = true)]
        public string product_id { get; set; }
        [DataMember(Name = "product_name", IsRequired = true)]
        public string product_name { get; set; }
        [DataMember(Name = "product_category", IsRequired = true)]
        public string product_category { get; set; }
        [DataMember(Name = "sku_name", IsRequired = true)]
        public string sku_name { get; set; }

        [DataMember(Name = "dimensions", IsRequired = true)]
        public IDimensions Dimensions { get; set; }

        [DataMember(Name = "identifiers", IsRequired = true)]
        public IIdentifiers Identifiers { get; set; }


    }
}

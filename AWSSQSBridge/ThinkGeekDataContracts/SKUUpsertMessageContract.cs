namespace GameStop.SupplyChain.DataContracts.ThinkGeek
{
    public class SKUUpsertMessageContract
    {
        public string ID { get; set; }
        public string TransmitTime { get; set; }

        public MessageTarget Target { get; set; }

        public string MessageType { get; set; }
        public string SKU { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string SKUName { get; set; }

        public Dimensions Dimensions { get; set; }

        public Identifiers Identifiers { get; set; }
    }
}

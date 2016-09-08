using System.Runtime.Serialization;


namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    [DataContract]
    public class Identifiers : IIdentifiers
    {
        [DataMember(Name = "upc_1", IsRequired = true)]
        public string upc_1 { get; set; }
        [DataMember(Name = "upc_2", IsRequired = true)]
        public string upc_2 { get; set; }
        [DataMember(Name = "ean", IsRequired = true)]
        public string ean { get; set; }
        [DataMember(Name = "jan", IsRequired = true)]
        public string jan { get; set; }
        [DataMember(Name = "isbn", IsRequired = true)]
        public string isbn { get; set; }
        [DataMember(Name = "warehouse_barcode", IsRequired = true)]
        public string warehoue_barcode { get; set; }

        public Identifiers(bool initDefaults = false)
        {
            if(initDefaults)
            {
                upc_1 = "618480236309";
                upc_2 = "";
                ean = "";
                jan = "";
                isbn = "";
                warehoue_barcode = "0EDD1H";
            }
        }
    }
}

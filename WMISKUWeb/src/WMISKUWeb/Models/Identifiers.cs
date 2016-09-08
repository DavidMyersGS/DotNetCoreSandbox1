using System.Runtime.Serialization;


namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public class Identifiers
    {
        public string upc_1 { get; set; }
        public string upc_2 { get; set; }
        public string ean { get; set; }
        public string jan { get; set; }
        public string isbn { get; set; }
        public string warehoue_barcode { get; set; }

        public Identifiers()
        {

        }

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

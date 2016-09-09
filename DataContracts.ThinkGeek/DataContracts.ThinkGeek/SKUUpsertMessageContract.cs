namespace GameStop.SupplyChain.DataContracts.ThinkGeek.SKUUpsert
{
    public class SKUUpsertMessageContract
    {
        public string GUID { get; set; }
        public string TransmitTime { get; set; }

        public MessageMetadata MessageMetadata { get; set; }

        public string MessageType { get; set; }
        public string SKU { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string SKUName { get; set; }

        public SKUCopy SKUCopy { get; set; }

        public Dimensions dimensions { get; set; }

        public Layout layout { get; set; }

        public Storefront storefront { get; set; }

        public Timestamps timestamps { get; set; }

        public Identifiers identifiers { get; set; }

        public Variant variant { get; set; }

        public string CountryOfOrigin { get; set; }
        public bool ProhibitAirShipment { get; set; }

        public Pricing pricing { get; set; }

        public Warehouse warehouse { get; set; }

        public Taxation taxation { get; set;

        }
    }
}

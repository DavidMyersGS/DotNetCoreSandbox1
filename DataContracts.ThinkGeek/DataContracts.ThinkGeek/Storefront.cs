using System;

namespace GameStop.SupplyChain.DataContracts.ThinkGeek.SKUUpsert
{
    public class Storefront
    {
        public string out_of_stock_behavior { get; set; }
        public bool is_active { get; set; }
        public bool is_discontinued { get; set; }
        public bool is_returnable { get; set; }
        public DateTime stock_replenishment_eta { get; set; }
        public int maximum_per_order { get; set; }
    }
}

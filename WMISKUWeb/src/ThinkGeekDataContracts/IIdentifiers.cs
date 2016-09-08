namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface IIdentifiers
    {
        string upc_1 { get; set; }
        string upc_2 { get; set; }
        string ean { get; set; }
        string jan { get; set; }
        string isbn { get; set; }
        string warehoue_barcode { get; set; }
    }
}

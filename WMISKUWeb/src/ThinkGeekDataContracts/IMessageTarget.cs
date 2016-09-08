namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface IMessageTarget
    {
        string Company { get; set; }
        string Warehouse { get; set; }
        string Brand { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface ISKUUpsertMessageContract : IMessageContract
    {
        string sku { get; set; }
        string product_id { get; set; }
        string product_name { get; set; }
        string product_category { get; set; }
        string sku_name { get; set; }

        IDimensions Dimensions { get; set; }

        IIdentifiers Identifiers { get; set; }

        }
}

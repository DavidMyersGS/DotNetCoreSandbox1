using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface ISKUUpsertMessageContract : IMessageContract
    {
        string SKU { get; set; }
        string ProductID { get; set; }
        string ProductName { get; set; }
        string ProductCategory { get; set; }
        string SKUName { get; set; }

        IDimensions Dimensions { get; set; }

        IIdentifiers Identifiers { get; set; }

        }
}

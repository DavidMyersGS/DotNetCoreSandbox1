using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStop.SupplyChain.ThinkGeekDataContracts
{
    public interface IDimensions
    {
        string weight { get; set; }
        string weight_override { get; set; }
        string length { get; set; }
        string height { get; set; }
        string depth { get; set; }
        string volume { get; set; }
        string case_weight { get; set; }
        string case_length { get; set; }
        string case_height { get; set; }
        string case_depth { get; set; }
        string case_volume { get; set; }
    }
}

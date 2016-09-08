using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMISKUWeb.Models
{
    public interface IDataLayer
    {
        int SubmitSKU(string json);
    }
}

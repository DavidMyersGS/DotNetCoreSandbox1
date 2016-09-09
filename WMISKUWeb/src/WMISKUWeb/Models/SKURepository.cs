using System;
using System.Collections.Generic;
using GameStop.SupplyChain.DataContracts.ThinkGeek;

namespace GameStop.SupplyChain.WMISKUWeb.Models
{
    public class SKURepository : IRepository<SKUUpsertMessageContract>
    {
        public SKURepository()
        {

        }

        public IEnumerable<SKUUpsertMessageContract> List
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Insert(SKUUpsertMessageContract obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(SKUUpsertMessageContract obj)
        {
            throw new NotImplementedException();
        }

        public SKUUpsertMessageContract FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(SKUUpsertMessageContract obj)
        {
            throw new NotImplementedException();
        }
    }
}

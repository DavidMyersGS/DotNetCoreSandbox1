using System.Collections.Generic;

namespace GameStop.SupplyChain.WMISKUWeb.Models
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List { get; }

        void Create(T obj);
        void Delete(T obj);
        void Update(T obj);
        T FindById(int id);
    }
}

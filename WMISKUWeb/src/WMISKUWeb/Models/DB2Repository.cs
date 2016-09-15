using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;

namespace GameStop.SupplyChain.WMISKUWeb.Models
{
    public class DB2Repository<T> : IRepository<T> where T : class
    {
        private OdbcConnection _conn = new OdbcConnection();
        private OdbcCommand _cmd = new OdbcCommand();

        public DB2Repository(IConfiguration config)
        {
            _conn.ConnectionString = config["Database:ConnectionString"].ToString();
        }

        public IEnumerable<T> List
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Create(T obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public T FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}

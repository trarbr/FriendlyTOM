using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    internal class CustomerMapper : ASQLMapper<CustomerEntity>
    {
        #region Internal Methods

        internal CustomerMapper(string connectionString)
        {
            this.connectionString = connectionString;
            this.entityMap = new Dictionary<int, CustomerEntity>();
        }

        internal CustomerEntity Create(CustomerType type, string note, string name)
        {
            CustomerEntity customerEntity = new CustomerEntity(type, note, name);
            insert(customerEntity);

            return customerEntity;
        }

        internal List<CustomerEntity> ReadAll()
        {
            List<CustomerEntity> customers = selectAll();
            return customers;
        }

        internal void Update(CustomerEntity customer)
        {
            update(customer);
        }

        internal void Delete(CustomerEntity customer)
        {
            customer.Deleted = true;
            Delete(customer);
        }
        #endregion

        #region Protected Methods
        protected override string insertProcedureName
        {
            get { throw new NotImplementedException(); }
        }

        protected override string selectAllProcedureName
        {
            get { throw new NotImplementedException(); }
        }

        protected override string updateProcedureName
        {
            get { throw new NotImplementedException(); }
        }

        protected override CustomerEntity entityFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void addInsertParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            throw new NotImplementedException();
        }

        protected override void addUpdateParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

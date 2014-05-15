﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using DataAccess.Entities;
using DataAccess.Helpers;

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
            get { return StoredProcedures.CREATE_CUSTOMER; }
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_CUSTOMERS; }
        }

        protected override string updateProcedureName
        {
            get { return StoredProcedures.UPDATE_CUSTOMER; }
        }

        protected override CustomerEntity entityFromReader(SqlDataReader reader)
        {
            string note = (string)reader["Note"];
            string name = (string)reader["Name"];
            CustomerType type = (CustomerType)Enum.Parse(typeof(CustomerType), reader["Type"].ToString());
            int id = (int)reader["PartyId"];
            bool deleted = (bool)reader["Deleted"];
            DateTime lastmodified = (DateTime)reader["LastModified"];

            CustomerEntity customerEntity = new CustomerEntity(type, name, note);
            customerEntity.Name = name;
            customerEntity.Note = note;
            customerEntity.Type = type;
            customerEntity.Id = id;
            customerEntity.Deleted = deleted;
            customerEntity.LastModified = lastmodified;

            return customerEntity;
        }

        protected override void addInsertParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            addCustomerParameters(entity, parameters);
        }

        protected override void addUpdateParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            addCustomerParameters(entity, parameters);
        }
        #endregion

        private void addCustomerParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            SqlParameter parameter = new SqlParameter("@Name", entity.Name);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
        }
    }
}

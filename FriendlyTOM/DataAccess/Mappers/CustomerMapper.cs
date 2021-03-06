﻿/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            //creates a new Customer and calls insert to add it to the database
            CustomerEntity customerEntity = new CustomerEntity(type, note, name);
            insert(customerEntity);
            return customerEntity;
        }

        internal CustomerEntity Read(int id)
        {
            //Reads a specific customer from the database
            CustomerEntity customer;
            entityMap.TryGetValue(id, out customer);
            return customer;
        }

        internal List<CustomerEntity> ReadAll()
        {
            //Reads all customers from the database. 
            List<CustomerEntity> customers = selectAll();
            return customers;
        }

        internal void Update(CustomerEntity customer)
        {
            update(customer);
        }

        internal void Delete(CustomerEntity customer)
        {
            //Sets a customer to deletede and calls update. 
            customer.Deleted = true;
            update(customer);
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
            //Sets data from database to corresponding data type for usage in the program. 
            string note = (string) reader["Note"];
            string name = (string) reader["Name"];
            CustomerType type = (CustomerType) Enum.Parse(typeof(CustomerType), reader["Type"].ToString());
            string contactPerson = (string)reader["ContactPerson"];
            string email = (string)reader["Email"];
            string address = (string)reader["Address"];
            string phoneNo = (string)reader["PhoneNo"];
            string faxNo = (string)reader["FaxNo"];

            int id = (int) reader["PartyId"];
            bool deleted = (bool) reader["Deleted"];
            DateTime lastmodified = (DateTime) reader["LastModified"];

            //Uses the data is sets to create an object of a customer.
            CustomerEntity customerEntity = new CustomerEntity(type, name, note);
            customerEntity.Name = name;
            customerEntity.Note = note;
            customerEntity.Type = type;
            customerEntity.ContactPerson = contactPerson;
            customerEntity.Email = email;
            customerEntity.Address = address;
            customerEntity.PhoneNo = phoneNo;
            customerEntity.FaxNo = faxNo;

            customerEntity.Id = id;
            customerEntity.Deleted = deleted;
            customerEntity.LastModified = lastmodified;

            //returns the customer.
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

        #region Private Methods
        private void addCustomerParameters(CustomerEntity entity, SqlParameterCollection parameters)
        {
            //defines what parameters a customer shall have. 
            SqlParameter parameter = new SqlParameter("@Name", entity.Name);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
            parameter = new SqlParameter("@ContactPerson", entity.ContactPerson);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Email", entity.Email);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Address", entity.Address);
            parameters.Add(parameter);
            parameter = new SqlParameter("@PhoneNo", entity.PhoneNo);
            parameters.Add(parameter);
            parameter = new SqlParameter("@FaxNo", entity.FaxNo);
            parameters.Add(parameter);
        }
        #endregion
    }
}

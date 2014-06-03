// MM
using System.Data.SqlClient;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using DataAccess.Helpers;
using Common.Enums;
namespace DataAccess.Mappers
{
    internal class SupplierMapper : ASQLMapper<SupplierEntity>
    {
        #region Internal Constructor
        internal SupplierMapper(string connectionString)
        {
            this.connectionString = connectionString;
            //Creates new instance of the entity dictionary with suppliers.
            this.entityMap = new Dictionary<int, SupplierEntity>();
        }
        #endregion

        #region Internal Methods
        internal SupplierEntity Create(string name, string note, SupplierType type)
        {
            //Creates a new supplier with type name and note, and inserts to database.
            SupplierEntity supplierEntity = new SupplierEntity(type, note, name);
            insert(supplierEntity);
            return supplierEntity;
        }

        internal SupplierEntity Read(int id)
        {
            //Read a supplier from the database.
            SupplierEntity supplier;
            entityMap.TryGetValue(id, out supplier);
            return supplier;
        }

        internal List<SupplierEntity> ReadAll()
        {
            //Reads all suppliers from the database
            List<SupplierEntity> suppliers = selectAll();
            return suppliers;
        }
        
        internal void Update(SupplierEntity supplier)
        {
            //Calls the update method to add new info to a row in the database
            update(supplier);
        }

        internal void Delete(SupplierEntity supplier)
        {
            //Deletes the object, and updates it in the database.
            supplier.Deleted = true;
            update(supplier);
        }
        #endregion

        #region Protected Methods
        protected override string insertProcedureName
        {
            get { return StoredProcedures.CREATE_SUPPLIER; }
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_SUPPLIERS; }
        }

        protected override string updateProcedureName
        {
            get { return StoredProcedures.UPDATE_SUPPLIER; }
        }

        protected override SupplierEntity entityFromReader(SqlDataReader reader)
        {
            //inputs the data from the database into useable data in the program. 
            string name = (string) reader["Name"];
            string note = (string) reader["Note"];
            SupplierType type = (SupplierType) Enum.Parse(typeof (SupplierType), reader["Type"].ToString());
            string accountNo = (string) reader["AccountNo"];
            string accountName = (string) reader["AccountName"];
            string ownerId = (string) reader["OwnerId"];
            string bank = (string) reader["Bank"];
            AccountType accountType = (AccountType) Enum.Parse(typeof (AccountType), reader["AccountType"].ToString());

            int id = (int)reader["PartyId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            SupplierEntity supplierEntity = new SupplierEntity(type, note, name);
            supplierEntity.Id = id;
            supplierEntity.LastModified = lastModified;
            supplierEntity.Deleted = deleted;
            supplierEntity.AccountNo = accountNo;
            supplierEntity.AccountName = accountName;
            supplierEntity.OwnerId = ownerId;
            supplierEntity.AccountType = accountType;
            supplierEntity.Bank = bank;

            return supplierEntity;
        }

        protected override void addInsertParameters(SupplierEntity entity,
           SqlParameterCollection parameters)
        {
            //Calls method to add the data into respective parameter
            addSupplierParameters(entity, parameters);
        }

        protected override void addUpdateParameters(SupplierEntity entity,
           SqlParameterCollection parameters)
        {
            //Calls method to update the data in respective parameter
            addSupplierParameters(entity, parameters);
        }
        #endregion

        #region Private Methods
        private void addSupplierParameters(SupplierEntity entity,
           SqlParameterCollection parameters)
        {
            //Creates different parameters corresponding to data in the database
            SqlParameter parameter = new SqlParameter("@Name", entity.Name);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
            parameter = new SqlParameter("@AccountNo", entity.AccountNo);
            parameters.Add(parameter);
            parameter = new SqlParameter("@AccountName", entity.AccountName);
            parameters.Add(parameter);
            parameter = new SqlParameter("@OwnerId", entity.OwnerId);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Bank", entity.Bank);
            parameters.Add(parameter);
            parameter = new SqlParameter("@AccountType", entity.AccountType.ToString());
            parameters.Add(parameter);
        }
        #endregion
    }
}

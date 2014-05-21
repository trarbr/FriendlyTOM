using System.Data.SqlClient;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Helpers;
using Common.Enums;

namespace DataAccess.Mappers
{
    internal class SupplierMapper : ASQLMapper<SupplierEntity>
    {

        internal SupplierMapper(string connectionString)
        {
            this.connectionString = connectionString;
            this.entityMap = new Dictionary<int, SupplierEntity>();
        }

        internal SupplierEntity Create(string name, string note, SupplierType type)
        {
            SupplierEntity supplierEntity = new SupplierEntity(type, note, name);

            insert(supplierEntity);

            return supplierEntity;
        }

        internal SupplierEntity Read(int id)
        {
            SupplierEntity supplier;
            entityMap.TryGetValue(id, out supplier);

            return supplier;
        }

        internal List<SupplierEntity> ReadAll()
        {
            List<SupplierEntity> suppliers = selectAll();

            return suppliers;
        }
        
        internal void Update(SupplierEntity supplier)
        {
            update(supplier);
        }

        internal void Delete(SupplierEntity supplier)
        {
            supplier.Deleted = true;
            update(supplier);
        }

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
            addSupplierParameters(entity, parameters);
        }

        protected override void addUpdateParameters(SupplierEntity entity,
           SqlParameterCollection parameters)
        {
            addSupplierParameters(entity, parameters);
        }

        private void addSupplierParameters(SupplierEntity entity,
           SqlParameterCollection parameters)
        {
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
    }
}

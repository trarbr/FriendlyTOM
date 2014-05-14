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

        internal SupplierEntity Create(string name, string note, string paymentInfo, SupplierType type)
        {
            SupplierEntity supplierEntity = new SupplierEntity(paymentInfo, type, note, name);

            insert(supplierEntity);

            return supplierEntity;
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
            get { return StoredProcedures.READ_ALL_SUPPLIERS; }
        }

        protected override SupplierEntity entityFromReader(SqlDataReader reader)
        {
            string name = (string) reader["Name"];
            string note = (string) reader["Note"];
            string paymentInfo = (string) reader["PaymentInfo"];
            SupplierType type = (SupplierType) Enum.Parse(typeof (SupplierType), reader["Type"].ToString());

            int id = (int)reader["SupplierId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            SupplierEntity supplierEntity = new SupplierEntity(paymentInfo, type, note, name);
            supplierEntity.Name = name;
            supplierEntity.Note = note;
            supplierEntity.PaymentInfo = paymentInfo;
            supplierEntity.Type = type;
            supplierEntity.Id = id;
            supplierEntity.LastModified = lastModified;
            supplierEntity.Deleted = deleted;

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
            parameter = new SqlParameter("@PaymentInfo", entity.PaymentInfo);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
        }
    }
}

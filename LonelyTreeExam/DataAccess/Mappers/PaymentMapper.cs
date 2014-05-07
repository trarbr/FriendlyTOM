using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Helpers;

namespace DataAccess.Mappers
{
    internal class PaymentMapper : ASQLMapper<PaymentEntity>
    {
        internal PaymentMapper(string connectionString)
        {
            this.connectionString = connectionString;
            this.entityMap = new Dictionary<int, PaymentEntity>();
        }

        protected override string insertProcedureName
        {
            get {throw new Exception();}
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.ReadAllPayments; }
        }

        protected override string updateProcedureName
        {
            get { throw new Exception(); }
        }

        protected override PaymentEntity entityFromReader(SqlDataReader reader)
        {
            string paymentName = (string)reader["PaymentName"];
            int id = (int)reader["PaymentId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            return new PaymentEntity(paymentName, id, lastModified, deleted);
        }

        protected override void addInsertParameters(PaymentEntity entity, SqlParameterCollection parameters)
        {
            //Not added yet
            throw new NotImplementedException();
        }

        protected override void addUpdateParameters(PaymentEntity entity, SqlParameterCollection parameters)
        {
            //Not added yet
            throw new NotImplementedException();
        }

        internal List<PaymentEntity> ReadAll()
        {
            List<PaymentEntity> payments = selectAll();

            // Finalize before returning!

            return payments;
        }
    }
}

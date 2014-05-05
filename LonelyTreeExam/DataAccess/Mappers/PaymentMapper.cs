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
            get { return StoredProcedures.; }
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.; }
        }

        protected override string updateProcedureName
        {
            get { return StoredProcedures.; }
        }

        protected override PaymentEntity entityFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void addInsertParameters(PaymentEntity entity, SqlParameterCollection parameters)
        {
            throw new NotImplementedException();
        }

        protected override void addUpdateParameters(PaymentEntity entity, SqlParameterCollection parameters)
        {
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

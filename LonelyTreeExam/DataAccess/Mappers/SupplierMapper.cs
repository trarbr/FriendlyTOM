using System.Data.SqlClient;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mappers
{
    internal class SupplierMapper : ASQLMapper<SupplierEntity>
    {
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

        protected override SupplierEntity entityFromReader(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void addInsertParameters(SupplierEntity entity, SqlParameterCollection parameters)
        {
            throw new NotImplementedException();
        }

        protected override void addUpdateParameters(SupplierEntity entity, SqlParameterCollection parameters)
        {
            throw new NotImplementedException();
        }
    }
}

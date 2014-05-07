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

        internal List<PaymentEntity> ReadAll()
        {
            List<PaymentEntity> payments = selectAll();

            // Finalize before returning!

            return payments;
        }

        protected override string insertProcedureName
        {
            get {throw new Exception();}
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_PAYMENTS; }
        }

        protected override string updateProcedureName
        {
            get { throw new Exception(); }
        }

        protected override PaymentEntity entityFromReader(SqlDataReader reader)
        {
            DateTime dueDate = (DateTime)reader["DueDate"];
            decimal dueAmount = (decimal)reader["DueAmount"];
            DateTime paidDate = (DateTime)reader["PaidDate"];
            decimal paidAmount = (decimal)reader["PaidAmount"];
            bool paid = (bool)reader["Paid"];
            bool archived = (bool)reader["Archived"];
            string attachments = (string)reader["Attachments"];

            string responsible = (string)reader["Responsible"];
            string commissioner = (string)reader["Commissioner"];
            string status = (string)reader["status"];
            string note = (string)reader["note"];

            int id = (int)reader["PaymentId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, responsible,
                commissioner);
            paymentEntity.PaidDate = paidDate;
            paymentEntity.PaidAmount = paidAmount;
            paymentEntity.Paid = paid;
            paymentEntity.Archived = archived;
            foreach (string attachment in attachments.Split(';'))
            {
                paymentEntity.AddAttachment(attachment);
            }

            paymentEntity.Status = status;
            paymentEntity.Note = note;

            paymentEntity.Id = id;
            paymentEntity.LastModified = lastModified;
            paymentEntity.Deleted = deleted;

            return paymentEntity; 
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

    }
}

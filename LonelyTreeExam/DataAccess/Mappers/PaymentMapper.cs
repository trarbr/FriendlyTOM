using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Helpers;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess.Mappers
{
    internal class PaymentMapper : ASQLMapper<PaymentEntity>
    {
        #region Internal Methods
        /// <summary>
        /// PaymentMapper should....
        /// </summary>
        /// <param name="connectionString"></param>
        internal PaymentMapper(string connectionString)
        {
            this.connectionString = connectionString;
            this.entityMap = new Dictionary<int, PaymentEntity>();
        }

        internal PaymentEntity Create(DateTime dueDate, decimal dueAmount, IParty responsible,
            IParty commissioner, PaymentType type, string sale, int booking)
        {
            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, responsible, commissioner,
                type, sale, booking);

            insert(paymentEntity);

            return paymentEntity;
        }

        /// <summary>
        /// returns all thats read from the database
        /// </summary>
        /// <returns>payments</returns>
        internal List<PaymentEntity> ReadAll()
        {
            List<PaymentEntity> payments = selectAll();

            // Finalize before returning!

            return payments;
        }

        internal void Update(PaymentEntity payment)
        {
            update(payment);
        }

        internal void Delete(PaymentEntity payment)
        {
            payment.Deleted = true;
            Update(payment);
        }
        #endregion


        #region Protected Methods

        /// <summary>
        /// Gets the insert stored procedure for adding payment to the database
        /// </summary>

        protected override string insertProcedureName
        {
            get { return StoredProcedures.CREATE_PAYMENT; }
        }

        /// <summary>
        /// gets the readall stored procedure for reading all payments in the database
        /// </summary>
        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_PAYMENTS; }
        }
        /// <summary>
        /// gets the procedure for updating payment in the database
        /// </summary>
        protected override string updateProcedureName
        {
            get { return StoredProcedures.UPDATE_PAYMENT; }
        }

        /// <summary>
        /// puts the information from the database into corresponding
        /// var from the entity
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>paymentEntity</returns>
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
            string note = (string)reader["Note"];
            PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), reader["Type"].ToString());
            string sale = (string)reader["Sale"];
            int booking = (int)reader["Booking"];
            string invoice = (string)reader["Invoice"];

            int id = (int)reader["PaymentId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, responsible,
                commissioner, type, sale, booking);
            paymentEntity.PaidDate = paidDate;
            paymentEntity.PaidAmount = paidAmount;
            paymentEntity.Paid = paid;
            paymentEntity.Archived = archived;
            if (attachments != "")
            {
                foreach (string attachment in attachments.Split(';'))
                {
                    paymentEntity.AddAttachment(attachment);
                }
            }

            paymentEntity.Note = note;
            paymentEntity.Invoice = invoice;

            paymentEntity.Id = id;
            paymentEntity.LastModified = lastModified;
            paymentEntity.Deleted = deleted;

            return paymentEntity; 
        }

        /// <summary>
        /// Adds the required paymentParameters for Insert to parameters
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="entity, parameters"></param>
        protected override void addInsertParameters(PaymentEntity entity, 
            SqlParameterCollection parameters)
        {
            addPaymentParameters(entity, parameters);
        }

        /// <summary>
        /// Adds the required paymentParameters for Update to parameters
        /// </summary>
        /// <param name="update"></param>
        /// <param name="entity, parameters"></param>
        protected override void addUpdateParameters(PaymentEntity entity, 
            SqlParameterCollection parameters)
        {
            addPaymentParameters(entity, parameters);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Adds the most common parameters shared in Insert and Update
        /// </summary>
        /// <param name="addPaymentParameters"></param>
        /// <param name="entity, parameters"></param>
        private void addPaymentParameters(PaymentEntity entity,
            SqlParameterCollection parameters)
        {
            SqlParameter parameter = new SqlParameter("@DueDate", entity.DueDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@DueAmount", entity.DueAmount);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Paid", entity.Paid);
            parameters.Add(parameter);
            parameter = new SqlParameter("@PaidDate", entity.PaidDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@PaidAmount", entity.PaidAmount);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Archived", entity.Archived);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Responsible", entity.Responsible);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Commissioner", entity.Commissioner);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Attachments", string.Join(";", entity.Attachments));
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
            parameter = new SqlParameter("@Sale", entity.Sale);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Booking", entity.Booking);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Invoice", entity.Invoice);
            parameters.Add(parameter);
        }
        #endregion

    }
}
